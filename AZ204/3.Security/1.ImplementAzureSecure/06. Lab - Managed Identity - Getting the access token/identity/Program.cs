using System.Net.Http.Headers;
using System.Text.Json;

string resource = "https://vault.azure.net";
string encodedResource = Uri.EscapeDataString(resource);
string requestUrl =
    $"http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource={encodedResource}";


using var httpClient = new HttpClient();

httpClient.DefaultRequestHeaders.Add("Metadata", "true");

HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

string json = await response.Content.ReadAsStringAsync();
ManagedIdentityTokenResponse? tokenResponse =
    JsonSerializer.Deserialize<ManagedIdentityTokenResponse>(json);
    

string accessToken = tokenResponse.access_token;
string tokenType = tokenResponse.token_type;
string expiresOn = tokenResponse.expires_on;

Console.WriteLine($"Token type: {tokenType}");
Console.WriteLine($"Expires on: {expiresOn}");
Console.WriteLine($"Access token: {accessToken}");

public record ManagedIdentityTokenResponse(
    string access_token,
    string token_type,
    string expires_on
);