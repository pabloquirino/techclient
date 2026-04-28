using Microsoft.AspNetCore.Mvc;
using TechClient.Application.DTOs;
using TechClient.Application.Services;

namespace TechClient.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatAppService _chatAppService;

    public ChatController(ChatAppService chatAppService)
    {
        _chatAppService = chatAppService;
    }

    [HttpPost("message")]
    public async Task<IActionResult> SendMessage([FromBody] ChatRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message cannot be empty.");

        if (string.IsNullOrWhiteSpace(request.SessionId))
            request.SessionId = Guid.NewGuid().ToString();

        var response = await _chatAppService.HandleMessageAsync(request);
        return Ok(response);
    }
}