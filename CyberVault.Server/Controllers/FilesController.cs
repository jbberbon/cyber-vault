using System.Security.Claims;
using CyberVault.Server.DTO.BlobFile;
using CyberVault.Server.Services.FilesService;
using Microsoft.AspNetCore.Authorization;
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

        // 03. Success
        if (response.IsSuccess) return Ok(response.BlobFileList);
        
        // 04. Error
        var hasError = response.Errors.Any();
        return hasError
            ? StatusCode(response.ErrorCode, new { errors = response.Errors })
            : StatusCode(response.ErrorCode);
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

        // 03. Success
        if (response.IsSuccess) return Created();

        // 04. Error
        var hasError = response.Errors.Any();
        return hasError
            ? StatusCode(response.ErrorCode, new { errors = response.Errors })
            : StatusCode(response.ErrorCode);
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

        // 03. Success
        if (response.IsSuccess) return Ok(new { sasUrl = response.SasUrl });

        // 04. Error
        var hasError = response.Errors.Any();
        return hasError
            ? StatusCode(response.ErrorCode, new { errors = response.Errors })
            : StatusCode(response.ErrorCode);
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

        // 03. Success
        if (response.IsSuccess) return Ok();

        // 04. Error
        var hasError = response.Errors.Any();
        return hasError
            ? StatusCode(response.ErrorCode, new { errors = response.Errors })
            : StatusCode(response.ErrorCode);
    }
}