using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TeenControlSystemWeb.Helpers;

public class JwtHelper
{
    public string GenerateJwt(string secret, long userId)
    {
        var jwtToken = new JwtSecurityToken(issuer: "TcsNeoApiServer",
            audience: "TcsNeoClient",
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                SecurityAlgorithms.HmacSha256),
            claims: new []{new Claim("id", userId.ToString())});
        var handler = new JwtSecurityTokenHandler();

        return handler.WriteToken(jwtToken);
    }

    public JwtResult ParseJwt(string secret, string jwt)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var secretBytes = Encoding.ASCII.GetBytes(secret);

        jwtHandler.ValidateToken(jwt, new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
            ValidateIssuer = true,
            ValidIssuer = "TcsNeoApiServer",
            ValidAudience = "TcsNeoClient",
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero
        }, out var token);
        
        var jwtToken = (JwtSecurityToken)token;

        var id = long.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

        return new JwtResult()
        {
            UserId = id
        };
    }
}

public class JwtResult
{
    public long UserId { get; init; }
}