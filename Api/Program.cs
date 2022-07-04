using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Integration.Swagger;
using Integration.JWT;
using Integration.Service.StartUp;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var xmlnames = new string[] { "UCmember.Api.xml", "UCmember.Entity.xml", "UCmember.Dto.xml" };
builder.Services.AddSwaggerConfiguration(xmlnames, "UCmember");
DependencyInjection.AddDependency(builder.Services);
DependencyInjection.AddPolicy(builder.Services);
builder.Services.AddIntegrationAuth();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseGateSwagger("UCmember");
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseGatewayAuthHeader();

app.UseAuthentication();

app.UseAuthorization();

//app.UseTokenAuth();

app.UseAuthSession();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
