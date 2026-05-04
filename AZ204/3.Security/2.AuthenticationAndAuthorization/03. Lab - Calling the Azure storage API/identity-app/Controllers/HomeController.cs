using Azure;
using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using identity_app.Models;

namespace identity_app.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ITokenAcquisition _tokenAcquisition;
     public HomeController(IConfiguration configuration, ITokenAcquisition tokenAcquisition)
    {
        _configuration = configuration;
        _tokenAcquisition = tokenAcquisition;
    }
    public async Task<IActionResult> Index()
    {
        string storageAccountName="stdeveus10";
        string containerName="data";
        string blobName="Dockerfile";

        string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(
                new[] { "https://storage.azure.com/user_impersonation" });

         var blobUri = new Uri(
                $"https://{storageAccountName}.blob.core.windows.net/{containerName}/{blobName}");

         var credential = new UserAccessTokenCredential(accessToken);
         var blobClient = new BlobClient(blobUri, credential);
         var download = await blobClient.DownloadContentAsync();
         string blobContent = download.Value.Content.ToString();

            ViewBag.BlobContent = blobContent;
            ViewBag.BlobName = blobName;

        return View();
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
