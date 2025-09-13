using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class FbrApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _bearerToken;

    public FbrApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        var fbrConfig = configuration.GetSection("FbrApi");
        _httpClient.BaseAddress = new Uri(fbrConfig["BaseUrl"]);
        _bearerToken = fbrConfig["BearerToken"];
    }

    public async Task<HttpResponseMessage> PostAsync(string relativeUrl, string jsonContent)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
        return await _httpClient.SendAsync(request);
    }
}