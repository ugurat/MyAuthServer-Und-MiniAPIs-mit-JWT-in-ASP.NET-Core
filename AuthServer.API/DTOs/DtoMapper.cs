using AuthServer.API.DATA;
using AutoMapper;

namespace AuthServer.API.DTOs
{
    public class DtoMapper : Profile // EINTRAGEN
    {

        public DtoMapper()
        {

            CreateMap<UserAppDto, UserApp>().ReverseMap();
            //CreateMap<ProductDto, Product>().ReverseMap();

        }

    }
}
