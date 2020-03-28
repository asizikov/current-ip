using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace CurrentIp.Web.IntegrationTests
{
    public class IntegrationTestSuite
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        
        public IntegrationTestSuite()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Testing");
            _server = new TestServer(builder);
            _client = _server.CreateClient();
        }

        [Fact]
        private async Task Health()
        {
            var httpResponseMessage = await _client.GetAsync("/api/currentip/latest").ConfigureAwait(false);
            
        }
    }
}