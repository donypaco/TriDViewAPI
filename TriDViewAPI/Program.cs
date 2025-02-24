
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TriDViewAPI.Data;
using TriDViewAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFeatureManagement();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DbConnection"]));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var appServices = typeof(Program).Assembly.GetTypes()
    .Where(t => t.Name.EndsWith("Service") && !t.IsInterface && !t.IsAbstract)
    .ToList();

foreach (var service in appServices)
{
    var serviceInterface = service.GetInterfaces().FirstOrDefault(i => i.Name == $"I{service.Name}");
    if (serviceInterface != null)
    {
        builder.Services.AddScoped(serviceInterface, service);
    }
}

var appRepositories = typeof(Program).Assembly.GetTypes()
    .Where(t => t.Name.EndsWith("Repository") && !t.IsInterface && !t.IsAbstract)
    .ToList();
foreach (var repository in appRepositories)
{
    var repositoryInterface = repository.GetInterfaces().FirstOrDefault(i => i.Name == $"I{repository.Name}");
    if (repositoryInterface != null)
    {
        builder.Services.AddScoped(repositoryInterface, repository);
    }
}

builder.Services.AddCors(p => p.AddPolicy("AllowAnyOrigin",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

builder.Configuration.AddJsonFile("appsettings.Development.json");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("JwtPolicy", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAnyOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
