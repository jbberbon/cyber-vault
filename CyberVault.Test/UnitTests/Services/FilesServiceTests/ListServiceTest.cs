/*using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CyberVault.Server.DTO.BlobFile;
using CyberVault.Server.Miscs.Constants;
using CyberVault.Server.Services;
using CyberVault.Server.Services.FilesService;
using Moq;

namespace CyberVault.Test.UnitTests.Services.FilesServiceTests;

public class ListServiceTest
{
    private IFilesService _filesService;

    // Mocked
    private Mock<IAzureBlobService> _azureBlobServiceMock;
    private Mock<BlobContainerClient> _containerClientMock;

    public ListServiceTest()
    {
        _azureBlobServiceMock = new Mock<IAzureBlobService>();
        _containerClientMock = new Mock<BlobContainerClient>();

        // Setup the FilesContainerClient property to return our mocked container client
        _azureBlobServiceMock.Setup(service => service.BlobContainerClient)
            .Returns(_containerClientMock.Object);

        // Initialize your service with the mock
        _filesService = new FilesService(_azureBlobServiceMock.Object);
    }


    private void SetupMocksForFailedBlobResponse(int statusCode)
    {
        var randomMessage = $"Error {statusCode}";

        // Create an AsyncPageable<BlobItem> that throws when enumerated
        var pageable = new Mock<AsyncPageable<BlobItem>>();

        // Set up the GetAsyncEnumerator to throw the exception
        _containerClientMock.Setup(client =>
                client.GetBlobsAsync(
                    It.IsAny<BlobTraits>(),
                    It.IsAny<BlobStates>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .Throws(new RequestFailedException(statusCode, randomMessage));
    }


    [Theory]
    [InlineData(404)]
    [InlineData(500)]
    [InlineData(409)]
    public async Task ListAsync_ShouldReturnErrorCode_AsExpected(int errorCode)
    {
        // Arrange
        SetupMocksForFailedBlobResponse(errorCode);

        // Act
        BlobFileResponseDto response = await _filesService.ListAsync();

        // Assert 
        Assert.False(response.IsSuccess);
        Assert.Equal(errorCode, response.ErrorCode);
    }
}*/