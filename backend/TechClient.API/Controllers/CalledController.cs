using Microsoft.AspNetCore.Mvc;
using TechClient.Application.DTOs;
using TechClient.Application.Services;

namespace TechClient.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalledController(CalledAppService calledAppService) : ControllerBase
{
    private readonly CalledAppService _calledAppService = calledAppService;

    [HttpPost]
    public async Task<IActionResult> OpenCalled([FromBody] OpenCalledDto dto)
    {
        var result = await _calledAppService.OpenCalledAsync(dto);
        return CreatedAtAction(nameof(GetByProtocol), new { protocol = result.Protocol }, result);
    }

    [HttpGet("{protocol}")]
    public async Task<IActionResult> GetByProtocol(string protocol)
    {
        var result = await _calledAppService.GetByProtocolAsync(protocol);
        return Ok(result);
    }
}