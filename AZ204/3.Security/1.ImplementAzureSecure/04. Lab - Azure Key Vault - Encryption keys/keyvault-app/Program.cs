using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System.Text;

string tenantId = "4f067c2d-2766-4c5f-a81a-ec27f14f8492";
string clientId = "0179d2b6-b1ff-4092-8496-f2df565c5ce5";
string clientSecret = "l-.8Q~Lrueu6IPSFRHBYxP3T61Nwbr7-pZMAEdo5";

var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

string vaultUrl="https://kv-dev-eus-100.vault.azure.net/";
string keyName ="kv-key";

var keyClient = new KeyClient(new Uri(vaultUrl), credential);
KeyVaultKey key;

key = await keyClient.GetKeyAsync(keyName);

string originalText = "Sensitive order data for CloudXeus Technology Services";
byte[] plaintextBytes = Encoding.UTF8.GetBytes(originalText);

var cryptoClient = new CryptographyClient(key.Id, credential);

EncryptResult encryptResult = await cryptoClient.EncryptAsync(
    EncryptionAlgorithm.RsaOaep,
    plaintextBytes);

Console.WriteLine("Encrypted text (Base64):");
Console.WriteLine(Convert.ToBase64String(encryptResult.Ciphertext));

DecryptResult decryptResult = await cryptoClient.DecryptAsync(
    EncryptionAlgorithm.RsaOaep,
    encryptResult.Ciphertext);

string decryptedText = Encoding.UTF8.GetString(decryptResult.Plaintext);

Console.WriteLine("\nDecrypted text:");
Console.WriteLine(decryptedText);