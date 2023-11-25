using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;
        private readonly IMapper _mapper;
        public ProductsController(IUnitOfWork UOW, IMapper mapper)
        {
            _uOW = UOW;
            _mapper = mapper;
        }

        [HttpGet("get-all-products")]
        public async Task<ActionResult> Get()
        {

            var products = await _uOW.ProductRepository.GetAllAsync(m => m.Category);
            var resluts = _mapper.Map<List<ProductDto>>(products);
            return Ok(resluts);

        }

        [HttpGet("get-product-by-id/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var product = await _uOW.ProductRepository.GetByIdAsync(id, m => m.Category);

            if (product is not null)
            {
                return Ok(_mapper.Map<Product, ProductDto>(product));

            }

            return BadRequest("Not Found");

        }

        [HttpPost("add-new-product")]

        public async Task<ActionResult> Post([FromForm] CreateProductDto productDto)
        {

            try
            {
                if (ModelState.IsValid)
                {
                  

                        var res = _mapper.Map<Product>(productDto);
                    await _uOW.ProductRepository.AddAsync(res);
                    return Ok(productDto);
               
                }

                return BadRequest(productDto);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        [HttpPut("update-exiting-product/{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] UpdateProductDto productDto)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uOW.ProductRepository.UpdateAsync(id, productDto);
                    return res ? Ok(productDto) : BadRequest();
                }
                return BadRequest(productDto);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }

            [HttpDelete("delete-exiting-product/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await _uOW.ProductRepository.DeleteAsyncWithPicture(id);
                    return res ? Ok(res) : BadRequest(res);
                }
                return NotFound($" This is {id} Not Found");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
