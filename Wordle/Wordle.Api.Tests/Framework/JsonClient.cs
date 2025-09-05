using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Wordle.Api.Tests.Framework
{
    public interface IJsonClient : IDisposable
    {
        HttpClient Client { get; }
        Task<TResponse> GetAsync<TResponse>(string uri, IDictionary<string, string?>? queryParams = null);
        Task<TResponse> PostAsync<TResponse>(string uri, IDictionary<string, string?>? queryParams = null);
        Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest request, IDictionary<string, string?>? queryParams = null);
        Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest request, IDictionary<string, string?>? queryParams = null);
        Task DeleteAsync(string uri, IDictionary<string, string?>? queryParams = null);
        void ClearCookies();
    }

    public class JsonClient : IJsonClient
    {
        public HttpClient Client { get; private set; }
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonClient(HttpClient client)
        {
            Client = client;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        private string BuildUri(string baseUri, IDictionary<string, string?>? queryParams)
        {
            if (queryParams == null || !queryParams.Any())
                return baseUri;

            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var param in queryParams.Where(p => p.Value != null))
            {
                query[param.Key] = param.Value;
            }

            return $"{baseUri}?{query}";
        }

        public async Task<TResponse> GetAsync<TResponse>(string uri, IDictionary<string, string?>? queryParams = null)
        {
            var response = await Client.GetAsync(BuildUri(uri, queryParams));
            HandleResponseCookies(response);
            return await GetResponseBody<TResponse>(response);
        }

        public async Task<bool> PostAsync(string uri, IDictionary<string, string?>? queryParams = null)
        {
            var response = await Client.PostAsync(BuildUri(uri, queryParams), null);
            HandleResponseCookies(response);
            return true;
        }

        public async Task<TResponse> PostAsync<TResponse>(string uri, IDictionary<string, string?>? queryParams = null)
        {
            var response = await Client.PostAsync(BuildUri(uri, queryParams), null);
            HandleResponseCookies(response);
            return await GetResponseBody<TResponse>(response);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest request, IDictionary<string, string?>? queryParams = null)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await Client.PostAsync(BuildUri(uri, queryParams), content);
            HandleResponseCookies(response);
            return await GetResponseBody<TResponse>(response);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest request, IDictionary<string, string?>? queryParams = null)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await Client.PutAsync(BuildUri(uri, queryParams), content);
            HandleResponseCookies(response);
            return await GetResponseBody<TResponse>(response);
        }

        public async Task<TResponse> PatchAsync<TRequest, TResponse>(string uri, TRequest request, IDictionary<string, string?>? queryParams = null)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(request, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await Client.PatchAsync(BuildUri(uri, queryParams), content);
            HandleResponseCookies(response);
            return await GetResponseBody<TResponse>(response);
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string uri, IDictionary<string, string?>? queryParams = null)
        {
            var response = await Client.DeleteAsync(BuildUri(uri, queryParams));
            HandleResponseCookies(response);
            return await GetResponseBody<TResponse>(response);
        }

        public async Task DeleteAsync(string uri, IDictionary<string, string?>? queryParams = null)
        {
            var response = await Client.DeleteAsync(BuildUri(uri, queryParams));
            HandleResponseCookies(response);
            ThrowIfNotSuccess(response);
        }

        private void HandleResponseCookies(HttpResponseMessage response)
        {
            response.Headers.TryGetValues("Set-Cookie", out var cookies);
            if (cookies != null)
            {
                Client.DefaultRequestHeaders.Remove("Cookie");
                Client.DefaultRequestHeaders.Add("Cookie", string.Join(";", cookies));
            }
        }

        public void ClearCookies()
        {
            Client.DefaultRequestHeaders.Remove("Cookie");
        }

        private async Task<TResponse> GetResponseBody<TResponse>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase, null, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(content, _jsonOptions)!;
        }

        private static void ThrowIfNotSuccess(HttpResponseMessage response, string? contentString = null)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Expected {HttpStatusCode.OK} but received {response.StatusCode}",
                    null,
                    response.StatusCode)
                {
                    Data =
                    {
                        { "ResponseBody", contentString },
                    },
                };
            }
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
