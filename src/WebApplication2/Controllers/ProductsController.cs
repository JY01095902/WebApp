using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    [Route("api/v1/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _productsRepository;
        public ProductsController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        [HttpGet]
        public async Task<ICollection<Product>> Get()
        {
            return await _productsRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<Product> Get(string id)
        {
            return await _productsRepository.GetAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            return Created(string.Empty, await _productsRepository.AddAsync(product));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productsRepository.DeleteAsync(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Id is not match.");
            }

            await _productsRepository.UpdateAsync(product);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody] IDictionary<string, object> patch)
        {
            if (patch.ContainsKey("Id") || patch.ContainsKey("id"))
            {
                return BadRequest("Id is not be patched.");
            }

            if (await _productsRepository.GetAsync(id) == null)
            {
                return NotFound();
            }

            await _productsRepository.PatchAsync(id, patch);

            return NoContent();
        }
    }
}
