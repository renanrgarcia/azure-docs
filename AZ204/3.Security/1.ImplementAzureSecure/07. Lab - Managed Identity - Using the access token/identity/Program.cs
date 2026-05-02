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

string vaultName = "kv-dev-eus-100";
string secretName = "SqlAdminPassword";
string secretUrl =
    $"https://{vaultName}.vault.azure.net/secrets/{secretName}?api-version=2025-07-01";

using var secretRequest = new HttpRequestMessage(HttpMethod.Get, secretUrl);
secretRequest.Headers.Authorization =
    new AuthenticationHeaderValue("Bearer", tokenResponse.access_token);

HttpResponseMessage secretHttpResponse = await httpClient.SendAsync(secretRequest);
string secretJson = await secretHttpResponse.Content.ReadAsStringAsync();
KeyVaultSecretResponse? secretResponse =
    JsonSerializer.Deserialize<KeyVaultSecretResponse>(secretJson);

Console.WriteLine($"Secret Id: {secretResponse.id}");
Console.WriteLine($"Secret Value: {secretResponse.value}");

public record ManagedIdentityTokenResponse(
    string access_token,
    string token_type,
    string expires_on
);

public record KeyVaultSecretResponse(
    string id,
    string value
);