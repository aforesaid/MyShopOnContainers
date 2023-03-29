using Microsoft.OpenApi.Models;
using Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "Web.API", Version = "v1"});
});            

var app = builder.Build();
app.Run();