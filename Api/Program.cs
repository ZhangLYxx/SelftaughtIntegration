using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Integration.Swagger;
using Integration.JWT;
using Integration.Service.StartUp;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Integration.ToolKits;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    options.SerializerSettings.Converters.Add(new LongConverter());
}); ;
builder.Services.AddEndpointsApiExplorer();
var xmlnames = new string[] { "UCmember.Api.xml", "UCmember.Entity.xml", "UCmember.Dto.xml" };
builder.Services.AddSwaggerConfiguration(xmlnames, "UCmember");
DependencyInjection.AddDependency(builder.Services);
DependencyInjection.AddPolicy(builder.Services);
builder.Services.AddIntegrationAuth();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();


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

app.UseAuthSession();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
