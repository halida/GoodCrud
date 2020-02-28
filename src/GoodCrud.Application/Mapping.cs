using System;
using AutoMapper;
using GoodCrud.Application.Contract.Dtos;
using Microsoft.Extensions.DependencyInjection;
using X.PagedList;

namespace GoodCrud.Application
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> func)
        {
            services.AddSingleton(CreateMapper(func));
        }

        public static IMapper CreateMapper(Action<IMapperConfigurationExpression> func)
        {
            var configuration = new MapperConfiguration((cfg) =>
            {
                cfg.AddProfile(new MappingProfile());
                func(cfg);
            });
            return configuration.CreateMapper();
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<PagedListMetaData, PagedListOpenMetaData>();
                CreateMap<PagedListOpenMetaData, PagedListMetaData>();
            }
        }

    }
}