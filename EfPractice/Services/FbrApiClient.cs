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


//{
//    "invoiceType": "Sale Invoice",
//  "invoiceDate": "2025-04-21",                // string, YYYY-MM-DD
//  "sellerNTNCNIC": "0786909",                 // string: 7- or 13-digit NTN or CNIC
//  "sellerBusinessName": "Your Company Name",
//  "sellerProvince": "Sindh",
//  "sellerAddress": "Karachi, Street / Address",

//  "buyerNTNCNIC": "1000000000000",            // string: buyer's NTN/CNIC (if registered) or placeholder if unregistered
//  "buyerBusinessName": "Buyer Name",
//  "buyerProvince": "Sindh",
//  "buyerAddress": "Buyer Address",
//  "buyerRegistrationType": "Registered",      // or "Unregistered"

//  "invoiceRefNo": "",                         // optional or empty string
//  "scenarioId": "SN001",                      // scenario identifier (as per FBR sandbox/production)
  
//  "items": [
//    {
//        "hsCode": "0101.2100",                  // string: HS code of product
//      "productDescription": "Product Name",
//      "uoM": "Numbers, pieces, units",       // unit of measure
//      "quantity": 1.0000,                     // number
//      "rate": "18%",                          // or numeric? doc shows string; but review sample carefully
//      "listPrice": 1000.00,                   // (optional / if required)
//      "valueSalesExcludingST": 1000.00,       // decimal
//      "salesTaxApplicable": 180.00,           // decimal
//      "fixedNotifiedValueOrRetailPrice": 0.00, // decimal, maybe optional
//      "salesTaxWithheldAtSource": 0.00,       // decimal, maybe optional
//      "extraTax": 0.00,                       // decimal, maybe optional
//      "totalValues": 1180.00                  // decimal: total including tax
//    }
//    // ... more items if necessary
//  ]
//}