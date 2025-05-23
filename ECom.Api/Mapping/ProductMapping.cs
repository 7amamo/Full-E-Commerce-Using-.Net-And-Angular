using AutoMapper;
using ECom.Core.DTO;
using ECom.Core.Entities.product;

namespace ECom.Api.Mapping
{
    public class ProductMapping : Profile
    {   
        public ProductMapping()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(p => p.CategryName,
                o => o.MapFrom
                (src => src.Category.Name)).ReverseMap();

            CreateMap<Photo, PhotoDTO>();

            CreateMap<AddProductDTO, Product>().ReverseMap()
                .ForMember(p => p.Photo, o => o.Ignore());


            CreateMap<UpdateProductDTO, Product>().ReverseMap()
               .ForMember(p => p.Photo, o => o.Ignore());
        }
    }
}
