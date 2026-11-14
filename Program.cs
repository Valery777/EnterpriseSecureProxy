using Application.DependencyInjection;
using EnterpriseSecureProxy.src.Api.Middleware;
using EnterpriseSecureProxy.src.Application.Configuration;
using EnterpriseSecureProxy.src.Infrastructure;
using EnterpriseSecureProxy.src.Infrastructure.DependencyInjection;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ---------- Logging ----------
builder.Host.UseSerilog((ctx, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration));

// ---------- Controllers ----------
builder.Services.AddControllers();

// ---------- Application ----------
builder.Services.AddApplication();

// ---------- Infrastructure ----------
builder.Services.AddInfrastructure(
    builder.Configuration);

// ---------- HTTP ----------
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// ---------- Cache ----------
builder.Services.AddMemoryCache();

builder.Services.Configure<CacheSettings>(
    builder.Configuration.GetSection(
        "CacheSettings"));

// ---------- Authentication ----------
builder.Services.AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
    });

// ---------- PostgreSQL ----------
builder.Services.AddDbContext<ProxyDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(
            "ProxyDb"));
});

// ----------  SQL Server----------
 
//builder.Services.AddDbContext<ProxyDbContext>(options =>
  //options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

// ---------- Swagger ----------
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Enterprise Secure Proxy",
            Version = "v1",
            Description = "Enterprise Secure Proxy"
        });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description =
                "Enter JWT token: Bearer {token}",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type =
                                ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});

// ---------- Build ----------
var app = builder.Build();

// ---------- Middleware ----------
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}