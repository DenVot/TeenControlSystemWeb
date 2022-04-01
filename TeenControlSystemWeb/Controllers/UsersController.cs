using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Extensions;

namespace TeenControlSystemWeb.Controllers;

[ApiController]
[Route("/api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IDataProvider _provider;

    public UsersController(IDataProvider provider)
    {
        _provider = provider;
    }
    
    [HttpGet("get-context-user")]
    public async Task<ActionResult> GetContextUser()
    {
        var user = await this.ExtractUserAsync(_provider);

        return Ok(user.ConvertToApiType());
    }

    [HttpGet("get-all-users")]
    public async Task<ActionResult> GetAllUsers()
    {
        var user = await this.ExtractUserAsync(_provider);

        if (!user.IsAdmin)
        {
            return BadRequest("У Вас нет прав на это действие");
        }

        return Ok(_provider.UsersRepository.GetAll().ToList().Select(x => x.ConvertToApiType()));
    }
}