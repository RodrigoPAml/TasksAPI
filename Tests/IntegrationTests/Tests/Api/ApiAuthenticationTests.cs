using Api.Base;
using System.Text;
using Newtonsoft.Json;
using IntegrationTests.Setup;

namespace IntegrationTests.Tests.Api
{
    /// <summary>
    /// Tests for authentication at api level
    /// </summary>
    [Collection("Integration tests")]
    public class ApiAuthenticationTests
    {
        private readonly WebApiSetup _setup;

        public ApiAuthenticationTests(WebApiSetup setup)
        {
            _setup = setup;
        }

        [Fact]
        public async Task TestLogin()
        {
            var loginRequest = new
            {
                Email = "email@email.com",
                Password = "password12345"
            };

            var requestContent = new StringContent(
                JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _setup.CreateHttpClient().PostAsync("authentication/login", requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<BaseResponse<string>>(responseContent);

            Assert.True(responseObj.Success);
        }
    }
}
