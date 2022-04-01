using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Providers;

namespace TeenControlSystemWeb.Controllers;

[ApiController]
[Route("/api/media")]
public class MediaController : ControllerBase
{
    private readonly MediaProvider _provider;

    public MediaController(MediaProvider provider)
    {
        _provider = provider;
    }

    [HttpGet("def-avatar")]
    public async Task<ActionResult> FetchAvatarBytes([FromQuery]long id)
    {
        try
        {
            return File(await _provider.GetUserAvatarAsync(id), "image/png", "defAvatar" + id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}