using System.Net.Http.Headers;
using System.Text;
using EfPractice.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

public class FbrApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IServiceProvider _sp;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string? _cachedBaseUrl;

    public FbrApiClient(HttpClient httpClient, IConfiguration configuration, IServiceProvider sp, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _sp = sp;
        _httpContextAccessor = httpContextAccessor;
        var fbrConfig = configuration.GetSection("FbrApi");
        _cachedBaseUrl = fbrConfig["BaseUrl"];
        if (!string.IsNullOrWhiteSpace(_cachedBaseUrl))
        {
            _httpClient.BaseAddress = new Uri(_cachedBaseUrl);
        }
    }

    private int GetCompanyId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId")?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }

    private async Task<string?> ResolveBearerTokenAsync()
    {
        var companyId = GetCompanyId();
        if (companyId == 0) return null;
        using var scope = _sp.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<StudentContext>();
        var comp = await ctx.Companies.AsNoTracking().FirstOrDefaultAsync(c => c.Id == companyId);
        return comp?.FbrBearerToken; // may be null if not set
    }

    public async Task<HttpResponseMessage> PostAsync(string relativeUrl, string jsonContent)
    {
        var token = await ResolveBearerTokenAsync();
        var request = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        return await _httpClient.SendAsync(request);
    }
}