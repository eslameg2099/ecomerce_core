using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IIProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context,
            IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateProductDto dto)
        {

            var filePath = "";
            if (dto.image is not null)
            {


                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolderPath))
                    Directory.CreateDirectory(uploadsFolderPath);

                 filePath = Path.Combine(uploadsFolderPath, dto.image.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.image.CopyToAsync(stream);
                }




            


            }





            //Create New Product
            var res = _mapper.Map<Product>(dto);
            res.ProductPicture = filePath;
            await _context.Products.AddAsync(res);
            await _context.SaveChangesAsync();
            return true;

        }


        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {

            var currentProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (currentProduct is not null)
            {
                var res = _mapper.Map<Product>(dto);
                res.Id = id;

                _context.Products.Update(res);
                await _context.SaveChangesAsync();


                return true;
            }

            return false;
         }


        public async Task<bool> DeleteAsyncWithPicture(int id)
        {

            var currentProduct = await _context.Products.FindAsync(id);

            if (currentProduct is not null)
            {
                //remove old picture
               

                //Remove
                _context.Products.Remove(currentProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }



    }
}
