using AutoMapper;
using ECom.Api.Helper;
using ECom.Core.DTO;
using ECom.Core.Entities.product;
using ECom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECom.Api.Controllers
{

    public class CategoryController : BaseController
    {
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories =await _unitOfWork.CategoryRepository.GetAllAsync();
                if (categories is null)
                {
                    return BadRequest(new ResponseApi(400));
                }
                return Ok(categories);
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
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category is null) {
                    return BadRequest(new ResponseApi(400,$"Not Found Categroy id = {id}"));
                }

                return Ok(category);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDTO);
                await _unitOfWork.CategoryRepository.AddAsync(category);
            
                return Ok(new ResponseApi(200, "item has been Added") );
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateCategoryDTO categoryDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDTO);

                await _unitOfWork.CategoryRepository.UpdateAsync(category);


                return Ok(new ResponseApi(200, "item has been Updated"));

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _unitOfWork.CategoryRepository.DeleteAsync(id);

                return Ok(new ResponseApi(200, "item has been Deleted"));


            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }
        }

    }
}
