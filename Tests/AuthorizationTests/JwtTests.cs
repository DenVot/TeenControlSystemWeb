using TeenControlSystemWeb.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Tests.AuthorizationTests;

public class JwtTests
{
    private readonly ITestOutputHelper _logger;

    public JwtTests(ITestOutputHelper output)
    {
        _logger = output;
    }
    
    [Fact]
    public void TestJwtTokenGeneration()
    {
        var jwt = new JwtHelper();

        var token = jwt.GenerateJwt(secret: "7536b1812b2fc0ca67a2cfd9466fdf9b", userId: 0L);
        Assert.False(string.IsNullOrEmpty(token));
        _logger.WriteLine(token);
    }

    [Fact]
    public void TestJwtParse()
    {
        var jwt = new JwtHelper();
        var token = jwt.GenerateJwt(secret: "7536b1812b2fc0ca67a2cfd9466fdf9b", userId: 0L);

        
        var jwtParseResult = jwt.ParseJwt("7536b1812b2fc0ca67a2cfd9466fdf9b", token);
        
        Assert.True(jwtParseResult.UserId == 0);
    }
}