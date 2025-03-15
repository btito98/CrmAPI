using AutoMapper;
using CRM.Application.DTOs.Cliente;
using CRM.Domain.Entities;

namespace CRM.Application.Mappings
{
    public static class AutoMapperHelper
    {
        public static void Configure(this IMapperConfigurationExpression config)
        {
            config.AutoMapperClientes();
        }

        public static void AutoMapperClientes(this IMapperConfigurationExpression config)
        {
            config.CreateMap<Cliente, ClienteCreateDTO>()
                .ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            config.CreateMap<Cliente, ClienteResultDTO>()
                .ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
