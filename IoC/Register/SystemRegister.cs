using Infra.Contexts;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Infra.Implementations.Email;
using Application.Interfaces.Email;
using Microsoft.EntityFrameworkCore;
using Infra.Implementations.Logging;
using Application.Interfaces.Logging;
using Domain.Interfaces.Authentication;
using Infra.Implementations.Security;
using Application.Interfaces.Security;
using Infra.Implementations.Transaction;
using Microsoft.Extensions.Configuration;
using Application.Interfaces.Transaction;
using Infra.Implementations.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Register
{
    /// <summary>
    /// Register system configrations and services
    /// </summary>
    public static class SystemRegister
    {
        public static void Invoke(IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(
                    Assembly.Load("Application")
                )
            );

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
            );

            services.AddDbContext<InfraContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
            );

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped<IUnityOfWork, UnityOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddSingleton<IHashService, HashService>();
            services.AddSingleton<IEmailService, EmailService>();
        }
    }
}
