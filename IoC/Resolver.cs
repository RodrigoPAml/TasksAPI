using IoC.Register;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC
{
    /// <summary>
    /// IoC Resolver
    /// </summary>
    public static class Resolver
    {
        public static void Invoke(IServiceCollection services, IConfiguration config)
        {
            MappingRegister.Invoke(services);
            RepositoryRegister.Invoke(services);
            BusinessRegister.Invoke(services);  
            AutoMapperRegister.Invoke(services);
            ValidationsRegister.Invoke(services);
            SystemRegister.Invoke(services, config);
            JwtRegister.Invoke(services, config);
            SwaggerRegisterGen.Invoke(services);
        }
    }
}