using AutoMapper;
using ColoursTest.Data.DTOs;
using ColoursTest.Data.Models;

namespace ColoursTest.Web
{
    public static class AutoMapperBuilder
    {
        public static void CreateMaps()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Person, PersonDetailsDto>();
                cfg.CreateMap<Person, PersonDto>()
                    .ForMember(dest => dest.PersonDetails, opts => opts.MapFrom(src => src));
            });
        }
    }
}