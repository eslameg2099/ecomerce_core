using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.Entities;

namespace Ecom.Core.Interfaces
{
    public interface IUnitOfWork
    {
     

        public ICategoryRepository CategoryRepository { get; }
        public IIProductRepository ProductRepository { get; }

      

    }
}
