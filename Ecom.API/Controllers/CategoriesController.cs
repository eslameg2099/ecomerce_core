using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Dtos;
using AutoMapper;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork UOW, IMapper mapper)
        {
            _uOW = UOW;
            _mapper = mapper;
        }

        [HttpGet("get-all-categories")]
        public async Task<ActionResult> Get()
        {
            var allCategories = _uOW.CategoryRepository.GetAll();

            if (allCategories is not null)
            {
                var res = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryDtocs>>((IReadOnlyList<Category>)allCategories);


                return Ok(res);

            }

            return BadRequest("Not Found");


        }

        [HttpGet("get-category-by-id/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var category = await _uOW.CategoryRepository.GetAsync(id);

            if (category is not null)
            {
                return Ok(_mapper.Map<Category, CategoryDtocs>(category));

            }

            return BadRequest("Not Found");

        }

        [HttpPost("add-new-category")]
        public async Task<ActionResult> Post(CategoryDtocs categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var res = _mapper.Map<Category>(categoryDto);

                    await _uOW.CategoryRepository.AddAsync(res);

                 return Ok(categoryDto);
                }

                return BadRequest(categoryDto);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpPut("update-exiting-category-by-id")]
        public async Task<ActionResult> Put(int id,CategoryDtocs categoryDto)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    var exitcatergoty = await _uOW.CategoryRepository.GetAsync(id);

                    if (exitcatergoty is not null)
                    {
                        _mapper.Map(categoryDto, exitcatergoty);
                        await _uOW.CategoryRepository.UpdateAsync(id, exitcatergoty);


                      
                        return Ok(categoryDto);

                    }


                }
                return BadRequest("Not Found");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpDelete("delete-category-by-id/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var exitingCategory = await _uOW.CategoryRepository.GetAsync(id);
                if (exitingCategory is not null)
                {
                    await _uOW.CategoryRepository.DeleteAsync(id);
                    return Ok($"This category [{exitingCategory.Name}] is deleted Successfully ");



                }
                return NotFound($"Category Not Found , Id [{id}] Incorrect");


            }

            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }


        }
}
