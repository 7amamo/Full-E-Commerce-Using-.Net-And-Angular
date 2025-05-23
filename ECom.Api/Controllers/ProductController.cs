using AutoMapper;
using ECom.Api.Helper;
using ECom.Core.DTO;
using ECom.Core.Entities.product;
using ECom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECom.Api.Controllers
{

    public class ProductController : BaseController
    {
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products =await _unitOfWork.ProductRepository
                    .GetAllAsync(p => p.Category, products => products.Photos);
                var MappedProduct = _mapper.Map<List<ProductDTO>>(products);

                if (products is null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                return Ok(MappedProduct);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository
                    .GetByIdAsync(id ,p => p.Category, products => products.Photos);
                var MappedProduct = _mapper.Map<ProductDTO>(product);

                if (product is null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                return Ok(MappedProduct);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }



        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductDTO productDTO)
        {
            try
            {

                //var MappedProduct = _mapper.Map<Product>(productDTO);
                await _unitOfWork.ProductRepository.AddProductAsync(productDTO);

                return Ok();


            }
            catch (Exception ex)
            {

                return Ok(new ResponseApi(400,ex.Message));
            }
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO productDTO)
        {
            try
            {
                await _unitOfWork.ProductRepository.UpdateProductAsync(productDTO);
                return Ok(new ResponseApi(200));
            }
            catch (Exception ex)
            {

                return Ok(new ResponseApi(400,ex.Message));

            }
        }


        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository
                    .GetByIdAsync(id, p=>p.Category,p=>p.Photos);

                await _unitOfWork.ProductRepository.DeleteProductAsync(product);
                return Ok(new ResponseApi(200));
            }
            catch (Exception ex)
            {

                return Ok(new ResponseApi(400, ex.Message));

            }
        }

    }
}
