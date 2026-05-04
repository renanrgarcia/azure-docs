using Azure;
using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using identity_app.Models;
using System.Text.Json;
using System.Net.Http.Headers;

namespace identity_app.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenAcquisition _tokenAcquisition;
     public HomeController(IHttpClientFactory httpClientFactory, ITokenAcquisition tokenAcquisition)
    {
        _httpClientFactory = httpClientFactory;
        _tokenAcquisition = tokenAcquisition;
    }
    public async Task<IActionResult> Index()
    {
       
        string[] scopes =
        {
            "api://deed110c-312a-49a4-9218-6f4c41b609dd/Orders.Read"
        };
      
        string ordersApiUrl = "https://orders-api-dev-eus-10.azurewebsites.net/api/orders";
        string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await httpClient.GetAsync(ordersApiUrl);
        await using var stream = await response.Content.ReadAsStreamAsync();

        var orders = await JsonSerializer.DeserializeAsync<List<Order>>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return View(orders ?? new List<Order>());        
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public sealed class UserAccessTokenCredential : TokenCredential
{
    private readonly string _accessToken;

    public UserAccessTokenCredential(string accessToken)
    {
        _accessToken = accessToken;
    }

    public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return new AccessToken(_accessToken, DateTimeOffset.UtcNow.AddMinutes(50));
    }

    public override ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(
            new AccessToken(_accessToken, DateTimeOffset.UtcNow.AddMinutes(50)));
    }
}
}
