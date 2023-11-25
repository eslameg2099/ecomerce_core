using AutoMapper;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        private readonly IFileProvider _fileProvider;

        public ICategoryRepository CategoryRepository { get; }

        public IIProductRepository ProductRepository { get; }

        public UnitOfWork(ApplicationDbContext context , IFileProvider fileProvider, IMapper mapper)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context, _fileProvider, _mapper);
        }

     
    }
}
