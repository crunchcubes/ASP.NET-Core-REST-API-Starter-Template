using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.IdentityModel.Tokens;
using Restful.Api;

namespace Restful.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                .UseStartup<StartupIntegrationTest>();

            _testServer = new TestServer(builder);            
            Client = _testServer.CreateClient();
            
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GenerateToken()}");
        }

        public string GenerateToken() 
        {
            var now = DateTime.UtcNow;
            var issuer = "issuer";
            var audience = "audience";
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(ClaimTypes.Name, "Dave", ClaimValueTypes.String, issuer));
            identity.AddClaim(new Claim("Role", "Administator", ClaimValueTypes.String, issuer));
            var signingCredentials = 
                new SigningCredentials(StartupIntegrationTest.ServerSecret, SecurityAlgorithms.HmacSha256);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(
                issuer, audience, identity, now, now.Add(TimeSpan.FromHours(1)), now, signingCredentials);
            var encodedJwt = handler.WriteToken(token);
            return encodedJwt;
        }

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
