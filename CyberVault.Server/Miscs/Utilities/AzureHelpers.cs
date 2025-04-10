namespace CyberVault.Server.Miscs.Utilities;

public static class AzureHelpers
{
    public static string ConstructBlobConnectionString(
        string blobAccountName,
        string blobAccountKey,
        string blobEndpointProtocol,
        string blobEndpoint
    )
    {
        // Construct the connection string using the provided values
        return
            $"DefaultEndpointsProtocol={blobEndpointProtocol};AccountName={blobAccountName};AccountKey={blobAccountKey};BlobEndpoint={blobEndpoint};";
    } 
}