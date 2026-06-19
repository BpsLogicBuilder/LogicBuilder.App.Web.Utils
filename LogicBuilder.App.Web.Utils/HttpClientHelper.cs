using LogicBuilder.App.Web.Utils.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogicBuilder.App.Web.Utils
{
    public class HttpClientHelper(IHttpClientFactory httpClientFactory) : IHttpClientHelper
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        private const string WEB_REQUEST_CONTENT_TYPE = "application/json";

        public async Task<TResult> GetAsync<TResult>(string url, JsonSerializerOptions? options = null)
        {
            HttpResponseMessage result;
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                result = await httpClient.GetAsync(url);
            }

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<TResult>
            (
                await result.Content.ReadAsStringAsync(),
                options
            ) ?? throw new InvalidOperationException($"Deserialization failed on {nameof(GetAsync)}.");
        }

        public async Task<TResult> PostAsync<TResult>(string url, string jsonObject, JsonSerializerOptions? options = null)
        {
            HttpResponseMessage result;
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                result = await httpClient.PostAsync(url, GetStringContent(jsonObject));
            }

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<TResult>
            (
                await result.Content.ReadAsStringAsync(),
                options
            ) ?? throw new InvalidOperationException($"Deserialization failed on {nameof(PostAsync)}.");
        }

        public async Task<TResult> PutAsync<TResult>(string url, string jsonObject, JsonSerializerOptions? options = null)
        {
            HttpResponseMessage result;
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                result = await httpClient.PutAsync(url, GetStringContent(jsonObject));
            }

            result.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<TResult>
            (
                await result.Content.ReadAsStringAsync(),
                options
            ) ?? throw new InvalidOperationException($"Deserialization failed on {nameof(PutAsync)}.");
        }

        private static StringContent GetStringContent(string jsonObject)
            => new
            (
                jsonObject,
                Encoding.UTF8,
                WEB_REQUEST_CONTENT_TYPE
            );
    }
}
