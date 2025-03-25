using Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests.Factories
{
    /// <summary>
    /// Web api factory with custom configuration for testing
    /// </summary>
    public class WebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.Tests.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            });
        }
    }
}