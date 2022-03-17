using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Attributes;
using TeenControlSystemWeb.Extensions;
using TeenControlSystemWeb.Helpers;
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
    
    [HttpPost("start-session")]
    public async Task<ActionResult> StartSession([FromQuery] long sessionId)
    {
        try
        {
            await _sessionProvider.StartSessionAsync(sessionId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("end-session")]
    public async Task<ActionResult> EndSession([FromQuery] long sessionId)
    {
        try
        {
            await _sessionProvider.EndSessionAsync(sessionId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("edit-session")]
    public async Task<ActionResult> EditSession([FromBody]SessionDelta delta, [FromQuery]long sessionId)
    {
        try
        {
            await _sessionProvider.EditSessionAsync(sessionId, delta);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("update-state")]
    public async Task<ActionResult> UpdateSessionState([FromQuery] long sessionId, [FromBody] SessionSnapshot snapshot)
    {
        try
        {
            await _sessionProvider.UpdateSessionStateAsync(sessionId, snapshot);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}