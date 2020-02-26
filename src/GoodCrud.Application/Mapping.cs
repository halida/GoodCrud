using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

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
                CreateMap<X.PagedList.PagedListMetaData, GoodCrud.Contract.Dtos.PagedListMetaData>();
                CreateMap<GoodCrud.Contract.Dtos.PagedListMetaData, X.PagedList.PagedListMetaData>();
            }
        }

    }
}