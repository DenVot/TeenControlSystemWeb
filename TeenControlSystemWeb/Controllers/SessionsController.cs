using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Extensions;
using TeenControlSystemWeb.Helpers;
using TeenControlSystemWeb.Providers;

namespace TeenControlSystemWeb.Controllers;

[ApiController]
//[Authorization]
[Authorize]
[Route("/api/sessions/")]
public class SessionsController : ControllerBase
{
    private readonly SessionProvider _sessionProvider;
    private readonly IDataProvider _dataProvider;

    public SessionsController(SessionProvider sessionProvider, IDataProvider dataProvider)
    {
        _sessionProvider = sessionProvider;
        _dataProvider = dataProvider;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> RegisterSession([FromBody] RegisterSessionType data)
    {
        var user = this.ExtractUserAsync(_dataProvider);

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
    public async Task<ActionResult> StartSession([FromQuery] long id)
    {
        try
        {
            await _sessionProvider.StartSessionAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("end-session")]
    public async Task<ActionResult> EndSession([FromQuery] long id)
    {
        try
        {
            await _sessionProvider.EndSessionAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("edit-session")]
    public async Task<ActionResult> EditSession([FromBody]SessionDelta delta, [FromQuery]long id)
    {
        try
        {
            await _sessionProvider.EditSessionAsync(id, delta);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("update-state")]
    public async Task<ActionResult> UpdateSessionState([FromQuery] long id, [FromBody] SessionSnapshot snapshot)
    {
        try
        {
            await _sessionProvider.UpdateSessionStateAsync(id, snapshot);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("get-session")]
    public async Task<ActionResult> GetSession([FromQuery]long id)
    {
        var session = await _sessionProvider.GetSessionAsync(id);

        if (session == null)
        {
            return BadRequest("Session not found");
        }

        return Ok(session.ConvertToApiType());
    }

    [HttpGet("get-active-sessions")]
    public ActionResult GetActiveSessions() => Ok(_sessionProvider.GetActiveSessions());
}