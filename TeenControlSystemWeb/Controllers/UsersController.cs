using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Attributes;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Extensions;

namespace TeenControlSystemWeb.Controllers;

[ApiController]
[Route("/api/users")]
[Authorization]
public class UsersController : ControllerBase
{
    private readonly IDataProvider _provider;

    public UsersController(IDataProvider provider)
    {
        _provider = provider;
    }
    
    [HttpGet("get-context-user")]
    public ActionResult GetContextUser()
    {
        var user = this.ExtractUser();

        return Ok(user.ConvertToApiType());
    }
}