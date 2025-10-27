using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LiveDWAPI.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AboutController : ControllerBase
{
    private readonly IMediator _mediator;

    public AboutController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Info")]
    public IActionResult GetInfo()
    {
        string? version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        return Ok(new{Service="live.dwapi.*",Version=version,Status="Running"});
    }
}