using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TeenControlSystemWeb.Data.Repositories;
using TeenControlSystemWeb.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TcsDatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("TcsDb"))
        .UseLazyLoadingProxies()
        .LogTo(System.Console.WriteLine,
            (eventId, logLevel) => logLevel > LogLevel.Information
                                   || eventId == RelationalEventId.CommandExecuting)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors());

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IDataProvider, DataProvider>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseMiddleware<JwtMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();