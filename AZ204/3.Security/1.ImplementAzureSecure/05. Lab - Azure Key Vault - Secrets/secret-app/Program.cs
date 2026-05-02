using Azure.Identity;
using System.Text;
using Azure.Security.KeyVault.Secrets;


string tenantId = "4f067c2d-2766-4c5f-a81a-ec27f14f8492";
string clientId = "0179d2b6-b1ff-4092-8496-f2df565c5ce5";
string clientSecret = "l-.8Q~Lrueu6IPSFRHBYxP3T61Nwbr7-pZMAEdo5";

var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

string vaultUrl="https://kv-dev-eus-100.vault.azure.net/";
string secretName = "SqlAdminPassword";

var secretClient = new SecretClient(new Uri(vaultUrl), credential);
KeyVaultSecret retrievedSecret = await secretClient.GetSecretAsync(secretName);
Console.WriteLine($"Retrieved secret name: {retrievedSecret.Name}");
Console.WriteLine($"Retrieved secret value: {retrievedSecret.Value}");
