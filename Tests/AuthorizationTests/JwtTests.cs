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

        var jwtParseResult = jwt.ParseJwt("7536b1812b2fc0ca67a2cfd9466fdf9b",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjAiLCJuYmYiOjE2NDY0NzM0NDksImV4cCI6MTY0NzY4MzA0OSwiaWF0IjoxNjQ2NDczNDQ5fQ.Pq2MWKEjJh3tEyFp-ARB0BuCIXpsSyb8HW1orTL2C4o");
        
        Assert.True(jwtParseResult.UserId == 0);
    }
}