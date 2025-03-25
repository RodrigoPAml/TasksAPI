using MediatR;
using IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Application.CQRS.Authentication.Login;
using Task = System.Threading.Tasks.Task;

namespace IntegrationTests.Tests.Application
{
    /// <summary>
    /// Tests for authentication at application level
    /// </summary>
    [Collection("Integration tests")]
    public class AppAuthenticationTests
    {
        private readonly WebApiSetup _setup;

        public AppAuthenticationTests(WebApiSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public async Task TestLogin()
        {
            using (var scope = _setup.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var mediator = provider.GetService<IMediator>();

                var result = await mediator.Send(new LoginCommand()
                {
                    Email = "email@email.com",
                    Password = "password12345"
                });

                Assert.True(result.Success);
            }
        }
    }
}