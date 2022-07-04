using Integration.HttpApi.Hosting;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Integration.JWT;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("ocelot.json", true, true);
})
.UseDefaultServiceProvider(providerOptions =>
{
    providerOptions.ValidateOnBuild = false;
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "Gateway API", Version = "v1", Description = "# gateway api..." });
});

//不使用consul服务，若使用即加一个Servcices.AddConsul
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseDeveloperExceptionPage();
var swaggers = builder.Configuration.GetSection("Swaggers").Get<SwaggerRouteInfo[]>();
if (swaggers != null)
{
    app.UseSwaggerUI(c =>
    {
        foreach (var swagger in swaggers)
        {
            c.SwaggerEndpoint(swagger.Url, swagger.Title);
        }
    });
}
app.UseRouting();
app.UseAuthorization();
app.MapSwagger();
app.UseTokenAuth();

app.UseOcelot().GetAwaiter();
app.Run();
