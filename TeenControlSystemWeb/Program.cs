using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using TeenControlSystemWeb.Data.Repositories;

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
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = "TcsNeoApiServer",
        ValidateAudience = true,
        ValidAudience = "TcsNeoClient",
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSecret"])),
        ValidateIssuerSigningKey = true
    };
});

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

//app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();