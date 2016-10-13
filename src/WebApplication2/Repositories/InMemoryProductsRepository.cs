using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public class InMemoryProductsRepository : IProductsRepository
    {
        private ICollection<Product> _products = new List<Product>
        {
            new Product { Id = "1", Name = "T-Shirt", Price = 100.00 },
            new Product { Id = "2", Name = "Shoes", Price = 399.00 },
            new Product { Id = "3", Name = "Cup", Price = 49.00 }
        };

        public Task<ICollection<Product>> GetAllAsync()
        {
            return Task.Run(() => _products);
        }

        public Task<Product> GetAsync(string id)
        {
            return Task.Run(() => _products.SingleOrDefault<Product>(p => p.Id == id));
        }

        public Task<Product> AddAsync(Product product)
        {
            product.Id = (Convert.ToInt64(_products.Max(p => p.Id)) + 1).ToString();
            _products.Add(product);

            return Task.Run(() => product);
        }

        public Task DeleteAsync(string id)
        {
            return Task.Run(() => _products.Remove(_products.SingleOrDefault<Product>(p => p.Id == id)));
        }

        public Task UpdateAsync(Product product)
        {
            _products.Remove(_products.SingleOrDefault<Product>(p => p.Id == product.Id));
            return Task.Run(() => _products.Add(product));
        }

        public Task PatchAsync(string id, IDictionary<string, object> patch)
        {
            return Task.Run(() =>
            {
                Product product = _products.SingleOrDefault<Product>(p => p.Id == id);
                foreach (var item in patch)
                {
                    string key = item.Key.Substring(0, 1).ToUpper() + item.Key.Substring(1);
                    PropertyInfo propertyInfo = typeof(Product).GetProperty(key);
                    propertyInfo.SetValue(product, item.Value);
                }
            });
        }
    }
}
