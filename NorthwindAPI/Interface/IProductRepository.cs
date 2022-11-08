﻿using NorthwindAPI.Model;

namespace NorthwindAPI.Interface
{
    public interface IProductRepository
    {
        Task<IReadOnlyList<Products>> GetAllProducts();

        Task<Products> GetProductById(int id);

        Task<Products> CreateProduct(ProductCreateDTO product);

        Task<Products> UpdateProduct(Products product);

        Task<Products> DeleteProduct(int id);
    }
}
