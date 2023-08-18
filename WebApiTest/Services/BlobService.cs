using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace WebApiTest.Services;

public interface IBlobService
{
    Task<(byte[] content, string contentType)> GetAsync(string blobName, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<string>> GetListAsync(CancellationToken cancellationToken);

    Task UploadAsync(string blobName, Stream content, string contentType, CancellationToken cancellationToken);

    Task DeleteAsync(string blobName, CancellationToken cancellationToken);
}

public class BlobService : IBlobService
{
    private readonly BlobContainerClient _containerClient;

    private const string _containerName = "test-blob-stor";

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
    }

    public async Task<(byte[] content, string contentType)> GetAsync(string blobName, CancellationToken cancellationToken)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        var response = await blobClient.DownloadContentAsync(cancellationToken);

        return (response.Value.Content.ToArray(), response.Value.Details.ContentType);
    }

    public async Task<IReadOnlyCollection<string>> GetListAsync(CancellationToken cancellationToken)
    {
        var blobItems = _containerClient.GetBlobsAsync(cancellationToken: cancellationToken);
        var blobNames = new List<string>();
        await Parallel.ForEachAsync<BlobItem>(blobItems, cancellationToken, (blobItem, token) =>
        {
            blobNames.Add(blobItem.Name);
            return ValueTask.CompletedTask;
        });

        return blobNames;
    }

    public async Task UploadAsync(string blobName, Stream content, string contentType, CancellationToken cancellationToken)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        content.Seek(0, SeekOrigin.Begin);
        _ = await blobClient.UploadAsync(
            content,
            new BlobHttpHeaders { ContentType = contentType },
            cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string blobName, CancellationToken cancellationToken)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        _ = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}
