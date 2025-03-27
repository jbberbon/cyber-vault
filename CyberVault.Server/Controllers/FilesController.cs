using System.Security.Claims;
using CyberVault.Server.DTO.BlobFile;
using CyberVault.Server.Services.FilesService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CyberVault.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFilesService _filesService;

    public FilesController(
        IFilesService filesService
    )
    {
        _filesService = filesService;
    }
    
    [HttpGet("list")]
    [Authorize]
    public async Task<IActionResult> List([FromQuery] GetListItemsDto request)
    {
        // 01. Retrieve and check user id if exists
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        // 02. Call ListAsync Service
        var response = await _filesService.ListAsync(userId, request.DirectoryId);

        // 03. Check if success
        if (!response.IsSuccess)
        {
            var hasError = response.Errors.Any();
            return hasError
                ? StatusCode(response.ErrorCode, new { errors = response.Errors })
                : StatusCode(response.ErrorCode);
        }

        // 04. Success
        return Ok(response.BlobFileList);
    }

    [HttpPost("upload")]
    [Authorize]
    public async Task<IActionResult> Upload([FromForm] UploadFileDto request)
    {
        // 01. Retrieve and check user id if exists
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        // 02. Call UploadAsync Service
        var response = await _filesService.UploadAsync(userId, request.File, request.ParentFolderId ?? "");
        if (!response.IsSuccess)
        {
            var hasError = response.Errors.Any();
            return hasError
                ? StatusCode(response.ErrorCode, new { errors = response.Errors })
                : StatusCode(response.ErrorCode);
        }

        // 03. Success
        return Created();
    }

    [HttpGet("download")]
    [Authorize]
    public async Task<IActionResult> Download([FromQuery] DownloadOrDeleteFileDto request)
    {
        // 01. Retrieve and check user id if exists
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        // 02. Call DownloadAsync Service
        var response = await _filesService.DownloadAsync(userId, request.FileName, request.ParentDirectoryId);
        if (!response.IsSuccess)
        {
            var hasError = response.Errors.Any();
            return hasError
                ? StatusCode(response.ErrorCode, new { errors = response.Errors })
                : StatusCode(response.ErrorCode);
        }

        try
        {
            // Ensure Content-Disposition is set to attachment to force the download
            var fileContent = response.BlobFile.Content!;
            var contentType = response.BlobFile.ContentType!;
            var fileName = response.BlobFile.Name;

            // Set Content-Disposition header to "attachment"
            Response.Headers["Content-Disposition"] = $"attachment; filename=\"{fileName}\"";

            // Set Content-Type to binary
            Response.ContentType = "application/octet-stream";

            // Return the file content to the user for download
            return File(fileContent, contentType, fileName);
        }
        catch (Exception e)
        {
            return StatusCode(500);
        }
    }

    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> Delete([FromBody] DownloadOrDeleteFileDto request)
    {
        // 01. Retrieve and check user id if exists
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        // 02. Call DeleteAsync Service
        var response = await _filesService.DeleteAsync(userId, request.FileName, request.ParentDirectoryId);

        if (!response.IsSuccess)
        {
            var hasError = response.Errors.Any();
            return hasError
                ? StatusCode(response.ErrorCode, new { errors = response.Errors })
                : StatusCode(response.ErrorCode);
        }

        // 03. Success
        return Ok();
    }
}