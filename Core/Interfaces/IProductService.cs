using Core.Entities;
using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateAsync(string name, string description, int rate);
        string CreateWithBackgroundJob(List<ProductsDto> products);
        Task<Product> GetByIdAsync(int id);

        Task<IReadOnlyList<Product>> GetAllAsync();
    }
}