﻿using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IIProductRepository:IGenericRepository<Product>
    {
        Task<bool> AddAsync(CreateProductDto dto);

        Task<bool> UpdateAsync(int id, UpdateProductDto dto);

        Task<bool> DeleteAsyncWithPicture(int id);


    }
}