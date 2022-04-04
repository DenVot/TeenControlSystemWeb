using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Services;

namespace TeenControlSystemWeb.Extensions;

public static class ControllerExtensions
{
    public static Task<User> ExtractUserAsync(this ControllerBase controllerBase, IDataProvider dataProvider)
    {
        var idStr = controllerBase.User.Claims.First(x => x.Type == "id").Value;
        var id = long.Parse(idStr);
        
        return dataProvider.UsersRepository.FindAsync(id)!;
    }

    public static bool IsUserIsAdmin(this ControllerBase controllerBase, RankService rankService)
    {
        var idStr = controllerBase.User.Claims.First(x => x.Type == "id").Value;
        var id = long.Parse(idStr);

        return rankService.IsAdmin(id);
    }
}