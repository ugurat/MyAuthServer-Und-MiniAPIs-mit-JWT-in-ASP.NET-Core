using AuthServer.API.DTOs;
using AutoMapper;

namespace AuthServer.API.Services
{
    public class ObjectMapper
    {

        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMapper>();
            });

            return config.CreateMapper();
        });

        public static IMapper Mapper => lazy.Value;


    }
}
