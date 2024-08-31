using CIDERS.Domain.Core.Db;
using CIDERS.Domain.Core.Repository.Cider;
using CIDERS.Domain.Injector;
using CIDERS.Domain.Provider.jwt;
using CIDERS.Domain.Utils;
using CIDERS.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;

// Configure DbContext
builder.Services.AddDbContext<CiderContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cider")));

// Register necessary services
builder.Services.AddTransient<DbContext, CiderContext>();

// Register the Functions service
builder.Services.AddScoped<Functions>();

// Register the IApiUserRepository service with its implementation
builder.Services.AddScoped<IApiUserRepository, ApiUserRepository>();
builder.Services.RegisterDependenciesCider();

builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

// Configure Authorization and Authentication
builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

// Configure JSON options to handle cyclic references
builder.Services.AddMvc()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/Log");

// CORS configuration
app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyOrigin();
    policyBuilder.AllowAnyMethod();
    policyBuilder.AllowAnyHeader();
});

// Middleware setup
app.UseExceptionHandleMiddleware();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();

app.MapControllers();

app.Run();
