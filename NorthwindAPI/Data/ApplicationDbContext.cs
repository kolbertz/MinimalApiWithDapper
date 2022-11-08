using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Interface;
using NorthwindAPI.Model;
using System.Data;

namespace NorthwindAPI.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public IDbConnection Connection => Database.GetDbConnection();

        public DbSet<Category> Categories { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Products> Products { get; set; }
    }
}
