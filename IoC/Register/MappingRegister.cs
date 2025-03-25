using Infra.Mappings;
using Infra.Interfaces.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Register
{
    /// <summary>
    /// Register database mappings
    /// </summary>
    public static class MappingRegister
    {
        public static void Invoke(IServiceCollection services)
        {
            services.AddSingleton<IMapping, UserMapping>();
            services.AddSingleton<IMapping, TaskMapping>();
            services.AddSingleton<IMapping, CategoryMapping>();
            services.AddSingleton<IMapping, VerificationCodeMapping>();
        }
    }
}