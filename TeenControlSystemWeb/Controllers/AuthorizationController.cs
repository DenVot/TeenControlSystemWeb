using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Attributes;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Exceptions.User;
using TeenControlSystemWeb.Extensions;
using TeenControlSystemWeb.Services;

namespace TeenControlSystemWeb.Controllers;

[ApiController]
[Route("/api/auth")]
[Produces("application/json")]
public class AuthorizationController : ControllerBase
{
    private readonly AuthorizationService _authorizationService;

    public AuthorizationController(IDataProvider dataProvider, IConfiguration configuration)
    {
        _authorizationService = new AuthorizationService(dataProvider, configuration);
    }

    [HttpPost]
    [Route("login")]
    public ActionResult Login([FromBody] LoginType loginType)
    {
        try
        {
            return Ok(_authorizationService.Login(loginType));
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }

    [HttpPost]
    [Authorization]
    public async Task<ActionResult> LogUp([FromBody] UserLogUpType logUpType)
    {
        if (!this.ExtractUser().IsAdmin)
        {
            return Unauthorized("У Вас нет прав для регистрации");
        }
        
        try
        {
            return Ok(await _authorizationService.LogUpAsync(logUpType));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}