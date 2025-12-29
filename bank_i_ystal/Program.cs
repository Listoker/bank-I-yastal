using bank_i_ystal.Middleware;
using bank_i_ystal.Repositories;
using bank_i_ystal.Repositories.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Account Service API",
        Version = "1984",
        Description = "Микросервис 'ну с богом' для управления банковскими счетами"
    });
});

builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account Service API 1984");
        c.DocExpansion(DocExpansion.None);
        c.InjectStylesheet("/swagger-ui/custom.css");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.UseStaticFiles();

app.MapGet("/swagger-ui/custom.css", async (HttpContext context) =>
{
    context.Response.ContentType = "text/css";
    await context.Response.WriteAsync(@"
        .swagger-ui {
            filter: invert(88%) hue-rotate(180deg);
        }
        
        .swagger-ui .scheme-container {
            background: #1a1a1a;
        }
        
        .swagger-ui .info .title {
            color: #ffffff;
        }
        
        .swagger-ui .opblock-tag {
            color: #ffffff;
        }
        
        body {
            background-color: #121212;
        }
        
        .swagger-ui img {
            filter: invert(100%) hue-rotate(180deg);
        }
        
        .swagger-ui .opblock .opblock-summary-description {
            color: #cccccc;
        }
        
        .topbar {
            background-color: #1a1a1a !important;
        }
        
        .swagger-ui .opblock.opblock-get {
            background: rgba(65, 150, 110, 0.1);
            border-color: #41966e;
        }
        
        .swagger-ui .opblock.opblock-post {
            background: rgba(73, 144, 226, 0.1);
            border-color: #4990e2;
        }
        
        .swagger-ui .opblock.opblock-put {
            background: rgba(255, 178, 43, 0.1);
            border-color: #ffb22b;
        }
        
        .swagger-ui .opblock.opblock-delete {
            background: rgba(249, 62, 62, 0.1);
            border-color: #f93e3e;
        }
        
        .swagger-ui .opblock.opblock-patch {
            background: rgba(80, 227, 194, 0.1);
            border-color: #50e3c2;
        }
        
        .swagger-ui .parameter__name {
            color: #e0e0e0;
        }
        
        .swagger-ui .parameter__type {
            color: #90caf9;
        }
        
        .swagger-ui table thead tr th, .swagger-ui table thead tr td {
            color: #ffffff;
            border-color: #333333;
        }
        
        .swagger-ui .model-title {
            color: #ffffff;
        }
        
        .swagger-ui .property-row {
            color: #cccccc;
        }
    ");
});

app.MapControllers();
app.Run();