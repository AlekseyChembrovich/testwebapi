using WebApiTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTest.Controllers;

[ApiController]
[Route("blobs")]
public class BlobController : ControllerBase
{
    private readonly IBlobService _blobService;

    public BlobController(IBlobService blobService)
	{
        _blobService = blobService;
    }

    [HttpGet("/blobs/{name}")]
    public async Task<IActionResult> Get(string name, CancellationToken cancellationToken)
    {
        (byte[] content, string contentType) = await _blobService.GetAsync(name, cancellationToken);

        return File(content, contentType);
    }

    [HttpGet("/blobs")]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken)
    {
        var blobNames = await _blobService.GetListAsync(cancellationToken);

        return Ok(blobNames);
    }

    [HttpPost("/blobs")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        MemoryStream memoryStream = new();
        await file.CopyToAsync(memoryStream, cancellationToken);
        await _blobService.UploadAsync(file.FileName, memoryStream, file.ContentType, cancellationToken);

        return NoContent();
    }

    [HttpDelete("/blobs")]
    public async Task<IActionResult> Delete(string name, CancellationToken cancellationToken)
    {
        await _blobService.DeleteAsync(name, cancellationToken);

        return NoContent();
    }
}
