using Core.Entities;
using Core.Interfaces;
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
        public string CreateWithBackgroundJob(string name, string description, int rate)
        {
            var jobid = backgroundJobs.Enqueue<ProductService>(x => x.insertProduct(name, description, rate));
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
        public void insertProduct(string name, string description, int rate)
        {
            var product = new Product(name, description, rate);
            _context.Products.Add(product);
            _context.SaveChanges();
        }
    }
}