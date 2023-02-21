using Autofac.Core;
using Integration.EntityFrameworkCore.DbMigrations.MySql;
using Integration.EntityFrameworkCore.DbMigrations.PGSql;
using Integration.EntityFrameworkCore.DbMigrations.SqlServer;
using Integration.JWT;
using Integration.Service.StartUp;
using Integration.Swagger;
using Integration.ToolKits;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Integration.Excel;

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
ServicesInitializer.Register(builder.Services, builder.Configuration);
DependencyInjection.AddDependency(builder.Services);
DependencyInjection.AddPolicy(builder.Services);
builder.Services.AddIntegrationAuth();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.AddExcelProvider();

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
