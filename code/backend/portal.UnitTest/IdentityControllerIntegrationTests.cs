using Microsoft.AspNetCore.Http;
using portal.Security.Identity;
using portal.Service;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace portal.UnitTest
{
    public class IdentityControllerIntegrationTests : IClassFixture<PortalServiceWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public IdentityControllerIntegrationTests(PortalServiceWebApplicationFactory<Program> factory)
            => _client = factory.CreateClient();

        [Fact]
        public async Task Login_Should_Pass()
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(new LoginRequest
            {
                Password = "P@$$w0rd",
                Username = "pub1@portal.com"
            }),
             Encoding.UTF8,
            "application/json");

            using HttpResponseMessage response = await _client.PostAsync(
                "api/identity/login",
                jsonContent);

            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        }

        [Fact]
        public async Task Login_Should_Fail()
        {
            using StringContent jsonContent = new(JsonSerializer.Serialize(new LoginRequest
            {
                Password = "P@",
                Username = "pub1@portal.com"
            }), Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _client.PostAsync(
                "api/identity/login",
                jsonContent);

            Assert.Equal(StatusCodes.Status401Unauthorized, (int)response.StatusCode);
        }
    }
}