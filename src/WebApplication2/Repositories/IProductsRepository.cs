using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public interface IProductsRepository
    {
        Task<ICollection<Product>> GetAllAsync();
        Task<Product> GetAsync(string id);
        Task<Product> AddAsync(Product product);
        Task DeleteAsync(string id);
        Task UpdateAsync(Product product);
        Task PatchAsync(string id, IDictionary<string, object> patch);
    }
}
