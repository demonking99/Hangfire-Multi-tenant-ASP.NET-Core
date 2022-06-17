using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Hangfire;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBackgroundJobClient backgroundJobs;
        public ProductService(ApplicationDbContext context, IBackgroundJobClient _backgroundJobs)
        {
            _context = context;
            backgroundJobs = _backgroundJobs;
        }

        public async Task<Product> CreateAsync(string name, string description, int rate)
        {
            var product = new Product(name, description, rate);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public string CreateWithBackgroundJob(List<ProductsDto> products)
        {
            var jobid = backgroundJobs.Enqueue<ProductService>(x => x.insertProduct(products));
            return jobid;
        }
        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public void insertProduct(List<ProductsDto> productsDTO)
        {
            List<Product> productsToInsert = new List<Product>();
            foreach (var productDto in productsDTO)
            {
                var product = new Product(productDto.Name, productDto.Description, productDto.Rate);
                productsToInsert.Add(product);
            }
            
            _context.Products.AddRange(productsToInsert);
            _context.SaveChanges();
        }
    }
}