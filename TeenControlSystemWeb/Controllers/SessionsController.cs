using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Attributes;
using TeenControlSystemWeb.Extensions;
using TeenControlSystemWeb.Providers;

namespace TeenControlSystemWeb.Controllers;

[ApiController]
[Authorization]
[Route("/api/sessions/")]
public class SessionsController : ControllerBase
{
    private readonly SessionProvider _sessionProvider;

    public SessionsController(SessionProvider sessionProvider)
    {
        _sessionProvider = sessionProvider;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> RegisterSession([FromBody] RegisterSessionType data)
    {
        var user = this.ExtractUser();

        try
        {
            await _sessionProvider.RegisterSessionAsync(user.Id,
                data.Name,
                data.StartAt,
                data.SensorsIds,
                data.FromPoint,
                data.ToPoint);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}