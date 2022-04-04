using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Extensions;
using TeenControlSystemWeb.Services;

namespace TeenControlSystemWeb.Controllers;

[ApiController]
[Route("/api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IDataProvider _provider;
    private readonly RankService _rankService;

    public UsersController(IDataProvider provider, RankService rankService)
    {
        _provider = provider;
        _rankService = rankService;
    }
    
    [HttpGet("get-context-user")]
    public async Task<ActionResult> GetContextUser()
    {
        var user = await this.ExtractUserAsync(_provider);

        return Ok(user.ConvertToApiType());
    }

    [HttpGet("get-all-users")]
    public ActionResult GetAllUsers()
    {
        if (!this.IsUserIsAdmin(_rankService))
        {
            return BadRequest("У Вас нет прав на это действие");
        }

        return Ok(_provider.UsersRepository.GetAll().ToList().Select(x => x.ConvertToApiType()));
    }
}