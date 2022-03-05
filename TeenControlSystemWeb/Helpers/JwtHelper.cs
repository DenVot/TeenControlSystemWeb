using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TeenControlSystemWeb.Helpers;

public class JwtHelper
{
    public string GenerateJwt(string secret, long userId)
    {
        var handler = new JwtSecurityTokenHandler();
        var codedSecret = Encoding.ASCII.GetBytes(secret);

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[] {new Claim("id", userId.ToString())}),
            Expires = DateTime.Now.AddDays(14),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(codedSecret), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }

    public JwtResult ParseJwt(string secret, string jwt)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var secretBytes = Encoding.ASCII.GetBytes(secret);

        jwtHandler.ValidateToken(jwt, new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
            ValidateIssuer = false,
            ValidateAudience = false,
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