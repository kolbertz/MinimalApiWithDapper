using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Interface;
using NorthwindAPI.Model;

namespace NorthwindAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public IApplicationDbContext _dbContext { get; }
        public IApplicationReadDbConnection _readDbContext { get; }
        public IApplicationWriteDbConnection _writeDbContext { get; }

        public ProductRepository(IApplicationDbContext dbContext, IApplicationReadDbConnection readDbConnection, IApplicationWriteDbConnection writeDbConnection)
        {
            _dbContext = dbContext;
            _readDbContext = readDbConnection;
            _writeDbContext = writeDbConnection;
        }

        public Task<IReadOnlyList<Products>> GetAllProducts()
        {
            var query = $"Select * from Products";
            return _readDbContext.QueryAsync<Products>(query);
        }

        public Task<IReadOnlyList<Products>> GetFilteredProductList(ProductFilter productFilter)
        {
            var baseQuery = "SELECT {0} FROM Products{1}";
            string selectClause = productFilter != null && productFilter.SelectProperties.Count > 0 ? string.Join(",", productFilter.SelectProperties) : "*";
            string whereClause = string.Empty;
            if (productFilter.CategoryId.HasValue)
            {
                whereClause += $" WHERE CategoryID = {productFilter.CategoryId}";
            }
            if (productFilter.SupplierId.HasValue)
            {
                if (string.IsNullOrEmpty(whereClause))
                {
                    whereClause += $" WHERE SupplierID = {productFilter.SupplierId}";
                }
                else
                {
                    whereClause += $" AND SupplierID = {productFilter.SupplierId}";
                }
            }
            if (!string.IsNullOrEmpty(productFilter.ProductName))
            {
                if (string.IsNullOrEmpty(whereClause))
                {
                    whereClause += $" WHERE ProductName = '{productFilter.ProductName}'";
                }
                else
                {
                    whereClause += $" AND ProductName = '{productFilter.ProductName}'";
                }
            }
            return _readDbContext.QueryAsync<Products>(string.Format(baseQuery, selectClause, whereClause));
        }

        public Task<Products> GetProductById(int id)
        {
            var query = $"SELECT * FROM Products WHERE ProductID = @ProductId";
            return _readDbContext.QueryFirstOrDefaultAsync<Products>(query, new { ProductId = id });
        }

        public async Task<Products> CreateProduct(ProductCreateDTO dto)
        {
            Products product = new Products(dto);
            _dbContext.Products.Add(product);
            if (await _dbContext.SaveChangesAsync(new CancellationToken()).ConfigureAwait(false) > 0)
            {
                return product;
            }
            else
            {
                return null;
            }
        }

        public async Task<Products> UpdateProduct(Products updateProduct)
        {
            Products oldProduct = await GetProductById(updateProduct.ProductId).ConfigureAwait(false);
            if (oldProduct != null)
            {
                _dbContext.Products.Update(updateProduct);
                if (await _dbContext.SaveChangesAsync(new CancellationToken()).ConfigureAwait(false) > 0)
                {
                    return updateProduct;
                }
            }
            return null;
        }

        public async Task<Products> DeleteProduct(int id)
        {
            Products deleteProduct = await GetProductById(id).ConfigureAwait(false);
            if (deleteProduct != null)
            {
                _dbContext.Products.Remove(deleteProduct);
                if (await _dbContext.SaveChangesAsync(new CancellationToken()).ConfigureAwait(false) > 0)
                {
                    return deleteProduct;
                }
            }
            return null;
        }
    }
}
