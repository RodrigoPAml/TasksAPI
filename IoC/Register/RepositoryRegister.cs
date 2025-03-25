using Domain.Interfaces.Repositories;
using Infra.Implementations.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Register
{
    /// <summary>
    /// Register repositories services
    /// </summary>
    public static class RepositoryRegister
    {
        public static void Invoke(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
        }
    }
}