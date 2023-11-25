using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;

namespace Ecom.API.MappingProfiles
{
    public class MappingCategory : Profile
    {
        public MappingCategory()
        {
            CreateMap<CategoryDtocs, Category>().ReverseMap();
         
        }
    }
}
