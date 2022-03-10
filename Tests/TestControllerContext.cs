using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace Tests;

public class TestControllerContext : ControllerContext
{
    public TestControllerContext() : base(new ActionContext(new DefaultHttpContext(), new RouteData(), new ControllerActionDescriptor()))
    {
        
    }
}