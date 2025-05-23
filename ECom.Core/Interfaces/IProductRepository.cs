using ECom.Core.DTO;
using ECom.Core.Entities.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<bool> AddProductAsync(AddProductDTO productDTO);
        public Task<bool> UpdateProductAsync(UpdateProductDTO productDTO);
        public Task DeleteProductAsync(Product product);


    }
}
