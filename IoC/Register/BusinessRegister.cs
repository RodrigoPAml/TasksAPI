using Domain.Business;
using Domain.Interfaces.Business;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Register
{
    /// <summary>
    /// Register the business services
    /// </summary>
    public static class BusinessRegister
    {
        public static void Invoke(IServiceCollection services)
        {
            services.AddScoped<IUserBusiness, UserBusiness>();
            services.AddScoped<ITaskBusiness, TaskBusiness>();
            services.AddScoped<ICategoryBusiness, CategoryBusiness>();
        }
    }
}