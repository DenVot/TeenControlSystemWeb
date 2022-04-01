using TeenControlSystemWeb.Extensions;
using Xunit;

namespace Tests.ExtensionsTests;

public class CommasTests
{
    [Fact]
    public void ConvertToCommasEnumerable()
    {
        var arr = new[] {1, 2, 3};
        
        Assert.Equal("1,2,3", arr.ConvertToCommasString());
    }
}