using AutoMapper;
using Books.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<BookCreateUpdateDto, Book>();
            CreateMap<Book, BookCreateUpdateDto>();
        }
    }
    public static class AutoMapperConfig
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var configuration = RegisterMappings();
            var mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
        }
    }

}