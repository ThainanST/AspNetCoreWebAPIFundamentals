using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CityInfo.Tests.integration
{
    public class ContentNegotiationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ContentNegotiationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetEndpoint_Returns406_WhenRequestingUnsupportedFormat()
        {
            // Arrange - Set Accept header to an unsupported format
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/cities");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/yaml")); // Unsupported format

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable); // Expect 406
        }
    }
}
