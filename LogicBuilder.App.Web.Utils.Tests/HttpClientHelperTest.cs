using LogicBuilder.App.Web.Utils.Interfaces;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LogicBuilder.App.Web.Utils.Tests
{
    public class HttpClientHelperTest
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly HttpClientHelper _httpClientHelper;

        public HttpClientHelperTest()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _httpClientHelper = new HttpClientHelper(_mockHttpClientFactory.Object);
        }

        #region GetAsync Tests

        [Fact]
        public async Task GetAsync_WithSuccessfulResponse_ReturnsDeserializedObject()
        {
            // Arrange
            var expectedResult = new TestModel { Id = 1, Name = "Test" };
            var jsonResponse = JsonSerializer.Serialize(expectedResult);
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, jsonResponse);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _httpClientHelper.GetAsync<TestModel>("https://api.example.com/test");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Name, result.Name);
        }

        [Fact]
        public async Task GetAsync_WithHttpError_ThrowsHttpRequestException()
        {
            // Arrange
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.NotFound, "Not Found");
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                async () => await _httpClientHelper.GetAsync<TestModel>("https://api.example.com/test")
            );
        }

        [Fact]
        public async Task GetAsync_WithInvalidJson_ThrowsJsonException()
        {
            // Arrange
            var invalidJson = "{ invalid json }";
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, invalidJson);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<JsonException>(
                async () => await _httpClientHelper.GetAsync<TestModel>("https://api.example.com/test")
            );
        }

        [Fact]
        public async Task GetAsync_WithNullResponse_ThrowsInvalidOperationException()
        {
            // Arrange
            var jsonResponse = "null";
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, jsonResponse);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _httpClientHelper.GetAsync<TestModel>("https://api.example.com/test")
            );
            Assert.Contains("Deserialization failed on GetAsync", exception.Message);
        }

        [Fact]
        public async Task GetAsync_WithCustomJsonOptions_UsesProvidedOptions()
        {
            // Arrange
            var expectedResult = new TestModel { Id = 1, Name = "Test" };
            var jsonResponse = JsonSerializer.Serialize(expectedResult, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, jsonResponse);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            // Act
            var result = await _httpClientHelper.GetAsync<TestModel>("https://api.example.com/test", options);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Name, result.Name);
        }

        #endregion

        #region PostAsync Tests

        [Fact]
        public async Task PostAsync_WithSuccessfulResponse_ReturnsDeserializedObject()
        {
            // Arrange
            var expectedResult = new TestModel { Id = 1, Name = "Test" };
            var jsonResponse = JsonSerializer.Serialize(expectedResult);
            var jsonRequest = JsonSerializer.Serialize(new { Name = "Test" });
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, jsonResponse);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _httpClientHelper.PostAsync<TestModel>("https://api.example.com/test", jsonRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Name, result.Name);
        }

        [Fact]
        public async Task PostAsync_WithHttpError_ThrowsHttpRequestException()
        {
            // Arrange
            var jsonRequest = JsonSerializer.Serialize(new { Name = "Test" });
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.BadRequest, "Bad Request");
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                async () => await _httpClientHelper.PostAsync<TestModel>("https://api.example.com/test", jsonRequest)
            );
        }

        [Fact]
        public async Task PostAsync_WithInvalidJson_ThrowsJsonException()
        {
            // Arrange
            var jsonRequest = JsonSerializer.Serialize(new { Name = "Test" });
            var invalidJson = "{ invalid json }";
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, invalidJson);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<JsonException>(
                async () => await _httpClientHelper.PostAsync<TestModel>("https://api.example.com/test", jsonRequest)
            );
        }

        [Fact]
        public async Task PostAsync_WithNullResponse_ThrowsInvalidOperationException()
        {
            // Arrange
            var jsonRequest = JsonSerializer.Serialize(new { Name = "Test" });
            var jsonResponse = "null";
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, jsonResponse);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _httpClientHelper.PostAsync<TestModel>("https://api.example.com/test", jsonRequest)
            );
            Assert.Contains("Deserialization failed on PostAsync", exception.Message);
        }

        [Fact]
        public async Task PostAsync_WithCustomJsonOptions_UsesProvidedOptions()
        {
            // Arrange
            var expectedResult = new TestModel { Id = 1, Name = "Test" };
            var jsonRequest = JsonSerializer.Serialize(new { Name = "Test" });
            var jsonResponse = JsonSerializer.Serialize(expectedResult, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, jsonResponse);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            // Act
            var result = await _httpClientHelper.PostAsync<TestModel>("https://api.example.com/test", jsonRequest, options);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Name, result.Name);
        }

        #endregion

        #region PutAsync Tests

        [Fact]
        public async Task PutAsync_WithSuccessfulResponse_ReturnsDeserializedObject()
        {
            // Arrange
            var expectedResult = new TestModel { Id = 1, Name = "Updated Test" };
            var jsonResponse = JsonSerializer.Serialize(expectedResult);
            var jsonRequest = JsonSerializer.Serialize(expectedResult);
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, jsonResponse);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _httpClientHelper.PutAsync<TestModel>("https://api.example.com/test/1", jsonRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Name, result.Name);
        }

        [Fact]
        public async Task PutAsync_WithHttpError_ThrowsHttpRequestException()
        {
            // Arrange
            var jsonRequest = JsonSerializer.Serialize(new { Id = 1, Name = "Test" });
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.InternalServerError, "Server Error");
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                async () => await _httpClientHelper.PutAsync<TestModel>("https://api.example.com/test/1", jsonRequest)
            );
        }

        [Fact]
        public async Task PutAsync_WithInvalidJson_ThrowsJsonException()
        {
            // Arrange
            var jsonRequest = JsonSerializer.Serialize(new { Id = 1, Name = "Test" });
            var invalidJson = "{ invalid json }";
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, invalidJson);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<JsonException>(
                async () => await _httpClientHelper.PutAsync<TestModel>("https://api.example.com/test/1", jsonRequest)
            );
        }

        [Fact]
        public async Task PutAsync_WithNullResponse_ThrowsInvalidOperationException()
        {
            // Arrange
            var jsonRequest = JsonSerializer.Serialize(new { Id = 1, Name = "Test" });
            var jsonResponse = "null";
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, jsonResponse);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _httpClientHelper.PutAsync<TestModel>("https://api.example.com/test/1", jsonRequest)
            );
            Assert.Contains("Deserialization failed on PutAsync", exception.Message);
        }

        [Fact]
        public async Task PutAsync_WithCustomJsonOptions_UsesProvidedOptions()
        {
            // Arrange
            var expectedResult = new TestModel { Id = 1, Name = "Updated Test" };
            var jsonRequest = JsonSerializer.Serialize(expectedResult);
            var jsonResponse = JsonSerializer.Serialize(expectedResult, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
            var mockHttpMessageHandler = CreateMockHttpMessageHandler(HttpStatusCode.OK, jsonResponse);
            using var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            // Act
            var result = await _httpClientHelper.PutAsync<TestModel>("https://api.example.com/test/1", jsonRequest, options);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.Name, result.Name);
        }

        #endregion

        #region Helper Methods

        private static Mock<HttpMessageHandler> CreateMockHttpMessageHandler(HttpStatusCode statusCode, string content)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                });

            return mockHttpMessageHandler;
        }

        #endregion

        #region Test Models

        private class TestModel
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        #endregion
    }
}
