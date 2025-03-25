using Respawn;
using Respawn.Graph;
using Infra.Contexts;
using Infra.Entities;
using IntegrationTests.Factories;
using Application.Interfaces.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Setup
{
    /// <summary>
    /// Setup a web api with a test database and data for testing
    /// </summary>
    public sealed class WebApiSetup : IDisposable
    {
        private readonly WebApplicationFactory _factory;
        private readonly DatabaseContext _context;

        private readonly IServiceProvider _provider;
        private readonly IServiceScope _scope;
        
        private Respawner _respawner = null;
        private string _connectionString = null;

        public WebApiSetup()
        {
            _factory = new WebApplicationFactory();
            
            _scope = _factory.Services.CreateScope();
            _provider = _scope.ServiceProvider;

            var config = _provider.GetService<IConfiguration>();

            _context = _provider.GetService<DatabaseContext>();
            _provider.GetService<InfraContext>(); // Wake up migrations
            _connectionString = config.GetConnectionString("DefaultConnection");

            _respawner = Respawner.CreateAsync(
                _connectionString,
                new RespawnerOptions()
                {
                    WithReseed = true,
                    TablesToIgnore = new[]
                    {
                       new Table("Migrations"),    
                    }
                }
            ).Result;

            _respawner.ResetAsync(_connectionString).Wait();

            SetupDatabase();
        }

        /// <summary>
        /// Its important to create a new scope for each test
        /// because it simulates a new request to the api
        /// </summary>
        /// <returns></returns>
        public IServiceScope CreateScope()
        {
            return _factory.Services.CreateScope();
        }   

        public HttpClient CreateHttpClient() => _factory.CreateClient();

        private void SetupDatabase()
        {
            using(var transaction = _context.Database.BeginTransaction())
            {
                var user = _context.Users.Add(new DbUser
                    {
                        Email = "email@email.com",
                        Password = _provider.GetService<IHashService>().Hash("password12345"),
                        Profile = 1,
                        Username = "TestUser"
                    }
                );

                _context.SaveChanges();

                var category = _context.Categories.Add(new DbCategory { Name = "TestCategory", UserId = user.Entity.Id });
                _context.SaveChanges();

                _context.Tasks.Add(new DbTask 
                    { 
                        Name = "TestTask",
                        CategoryId = category.Entity.Id,
                        UserId = user.Entity.Id,
                        Description = "This is a test",
                        Status = 1,
                        Priority = 1,
                        DueDate = DateTime.Now
                    }
                );

                _context.SaveChanges();
                transaction.Commit();
            }

            _scope.Dispose();
        }

        public void Dispose()
        {
            _respawner.ResetAsync(_connectionString).Wait(); 
            _factory.Dispose();
        }
    }
}
