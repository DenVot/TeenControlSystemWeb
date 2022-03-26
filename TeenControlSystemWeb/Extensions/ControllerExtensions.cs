using Microsoft.AspNetCore.Mvc;
using TeenControlSystemWeb.Data.Repositories;

namespace TeenControlSystemWeb.Extensions;

public static class ControllerExtensions
{
    public static Task<User> ExtractUserAsync(this ControllerBase controllerBase, IDataProvider dataProvider)
    {
        var idStr = controllerBase.User.Claims.First(x => x.Type == "id").Value;
        var id = long.Parse(idStr);
        
        return dataProvider.UsersRepository.FindAsync(id)!;
    }
}