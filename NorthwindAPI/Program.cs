using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using NorthwindAPI.Connections;
using NorthwindAPI.Data;
using NorthwindAPI.Interface;
using NorthwindAPI.Model;
using NorthwindAPI.Repositories;
using Swashbuckle.AspNetCore.Annotations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Northwind Test API with Dapper AND EFCore",
        Description = "This is an CashControl Test API using Microsofts Minimal API with Dapper and EF Core as ORM Mapper"
    });
    c.EnableAnnotations();
});

// Register custom services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
builder.Services.AddScoped<IApplicationWriteDbConnection, ApplicationWriteDbConnection>();
builder.Services.AddScoped<IApplicationReadDbConnection, ApplicationReadDbConnection>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(opt => {
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    //opt.RoutePrefix = "/swagger";
});

//app.UseHttpsRedirection();
app.MapGet("/products", async() =>
{
    IReadOnlyList<Products> productsList = null;
    using (var scope = app.Services.CreateScope())
    {
        productsList = await scope.ServiceProvider.GetRequiredService<IProductRepository>().GetAllProducts().ConfigureAwait(false);
    }
    return productsList;
}).WithMetadata(new SwaggerOperationAttribute("Method uses Dapper!"));

app.MapGet("/products/{id}", async(int id) =>
{
    Products products = null;
    using (var scope = app.Services.CreateScope())
    {
        products = await scope.ServiceProvider.GetRequiredService<IProductRepository>().GetProductById(id).ConfigureAwait(false);
    }
    return products;
}).WithMetadata(new SwaggerOperationAttribute("Method uses Dapper!"));

app.MapPost("/products/{product}", async(ProductCreateDTO product) => {
    Products products = null;
    using (var scope = app.Services.CreateScope())
    {
        products = await scope.ServiceProvider.GetRequiredService<IProductRepository>().CreateProduct(product).ConfigureAwait(false);
    }
    return products;
}).WithMetadata(new SwaggerOperationAttribute("Method uses EF Core!"));
app.MapPut("/products/{product}", async(Products product) => {
    Products products = null;
    using (var scope = app.Services.CreateScope())
    {
        products = await scope.ServiceProvider.GetRequiredService<IProductRepository>().UpdateProduct(product).ConfigureAwait(false);
    }
    return products;
}).WithMetadata(new SwaggerOperationAttribute("Method uses EF Core!"));
app.MapDelete("/products/{id}", async(int id) => {
    Products products = null;
    using (var scope = app.Services.CreateScope())
    {
        products = await scope.ServiceProvider.GetRequiredService<IProductRepository>().DeleteProduct(id).ConfigureAwait(false);
    }
    return products;
}).WithMetadata(new SwaggerOperationAttribute("Method uses EF Core!"));

app.Run();
