using System.Security.Claims;
using CyberVault.Server.DTO.BlobFile;
using CyberVault.Server.DTO.Directory;
using CyberVault.Server.Services.DirectoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberVault.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoldersController : ControllerBase
{
    private readonly IDirectoryService _directoryService;

    public FoldersController(
        IDirectoryService directoryService
    )
    {
        _directoryService = directoryService;
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateFolderDto request)
    {
        // 01. Retrieve and check user id if exists
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        // 02. Call CreateDirectory Service
        var response = await _directoryService.CreateAsync(userId, request.FolderName, request.ParentDirectoryId ?? "");

        // 03. Success
        if (response.IsSuccess) return Created();

        // 04. Error
        var hasError = response.Errors.Any();
        return hasError
            ? StatusCode(response.ErrorCode, new { errors = response.Errors })
            : StatusCode(response.ErrorCode);
    }

    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> Delete([FromBody] DeleteFolderDto request)
    {
        // 01. Retrieve and check user id if exists
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        // 02. Call DeleteDirectory Service
        var deleteRes = await _directoryService.DeleteAsync(userId, request.DirectoryId);

        // 03. Return 
        return deleteRes.IsSuccess ? Ok() : StatusCode(deleteRes.ErrorCode);
    }
}