using Microsoft.Extensions.DependencyInjection;
using Application.CQRS.Authentication.Login;

namespace IoC.Register
{
    /// <summary>
    /// Register the validations classes
    /// </summary>
    public static class ValidationsRegister
    {
        public static void Invoke(IServiceCollection services)
        {
            InvokeApp(services);
        }

        private static void InvokeApp(IServiceCollection services)
        {
            services.AddScoped<LoginValidator>();
        }
    }
}
