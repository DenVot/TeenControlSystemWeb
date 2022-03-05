using Microsoft.AspNetCore.Mvc;

namespace TeenControlSystemWeb.Extensions;

public static class ControllerExtensions
{
    public static User ExtractUser(this ControllerBase controllerBase)
    {
        return (User) controllerBase.HttpContext.Items["User"]!;
    }
}