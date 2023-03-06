using Microsoft.AspNetCore.Http;
using portal.Security.Identity;
using portal.Service;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace portal.UnitTest
{
    public class AuthorizeIntegrationTests : IClassFixture<PortalServiceWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthorizeIntegrationTests(PortalServiceWebApplicationFactory<Program> factory)
            => _client = factory.CreateClient();

        [Theory]
        [InlineData("api/journals")]
        [InlineData("api/journals/subscribed")]
        [InlineData("api/journals/published")]
        [InlineData("api/journals/f7086a17-dde4-40e5-9614-17c415364d6c")]
        [InlineData("api/Editions/f7086a17-dde4-40e5-9614-17c415364d6c/download")]
        [InlineData("api/Editions/f7086a17-dde4-40e5-9614-17c415364d6c")]
        public async Task Authorize_ShouldBe401(string url)
        {
            using HttpResponseMessage response = await _client.GetAsync(url);

            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
        }

        [Theory]
        [InlineData("api/journals")]
        [InlineData("api/journals/subscribed")]
        [InlineData("api/journals/published")]
        [InlineData("api/journals/f7086a17-dde4-40e5-9614-17c415364d6c")]
        [InlineData("api/Editions/f7086a17-dde4-40e5-9614-17c415364d6c/download")]
        [InlineData("api/Editions/f7086a17-dde4-40e5-9614-17c415364d6c")]
        public async Task Authorize_ShouldNotBe401(string url)
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(new LoginRequest
            {
                Password = "P@$$w0rd",
                Username = "pub1@portal.com"
            }),
         Encoding.UTF8,
        "application/json");

            using HttpResponseMessage tokenResponse = await _client.PostAsync(
                "api/identity/login",
                jsonContent);

            var jsonResponse = await tokenResponse.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true // this is the point
            };

            var token = JsonSerializer.Deserialize<TokenResponse>(jsonResponse, options);

            _client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", token.Token);

            using HttpResponseMessage response = await _client.GetAsync(url);

            Assert.NotEqual(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
        }
    }
}