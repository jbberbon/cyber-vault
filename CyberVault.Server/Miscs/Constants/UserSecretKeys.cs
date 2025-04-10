namespace CyberVault.Server.Miscs.Constants;

public class UserSecretKeys
{
    // Database
    public const string DefaultConnection = "DefaultConnection";

    // Blob Storage
    public const string BlobAccountName = "AzureStorageAccount:Blob:AccountName";
    public const string BlobAccountKey = "AzureStorageAccount:Blob:AccountKey";
    public const string BlobEndpointProtocol = "AzureStorageAccount:Blob:DefaultEndpointProtocol";
    public const string BlobEndpoint = "AzureStorageAccount:Blob:Endpoint";

    // TODO: DELETE LATER
    public const string BlobStorageAccount = "AzureBlob:BlobStorageAccount";
    public const string BlobKey = "AzureBlob:Key";
}