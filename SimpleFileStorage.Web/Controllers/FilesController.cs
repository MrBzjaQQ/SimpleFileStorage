using Microsoft.AspNetCore.Mvc;
using SimpleFileStorage.Web.AppServices.Contract;
using SimpleFileStorage.Web.Domain;

namespace SimpleFileStorage.Web.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService) => _fileService = fileService;

    [HttpPost("upload")]
    public async Task<ActionResult<StoredFile>> Upload([FromForm] IFormFile file, [FromForm] string fileName) =>
        Ok(await _fileService.UploadAsync(file, fileName));

    [HttpGet("{id}")]
    public async Task<IActionResult> Download(Guid id)
    {
        var file = await _fileService.DownloadAsync(id);
        return File(file.File, "application/octet-stream", file.FileName);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _fileService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<StoredFile>>> List([FromQuery] int take = 10, [FromQuery] int skip = 0, [FromQuery] string? searchTerm = null) =>
        Ok(await _fileService.GetListAsync(take, skip, searchTerm));
}