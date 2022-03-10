using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Helpers;

namespace TeenControlSystemWeb.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _secret;
    private readonly JwtHelper _jwt;
    
    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _jwt = new JwtHelper();
        _secret = configuration["JwtSecret"];
    }

    public async Task Invoke(HttpContext context, IDataProvider dataProvider)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var jwtResult = _jwt.ParseJwt(_secret, token);
            var userId = jwtResult.UserId;

            var targetUser = await dataProvider.UsersRepository.FindAsync(userId);
            context.Items["User"] = targetUser;
        }

        await _next(context);
    }
}