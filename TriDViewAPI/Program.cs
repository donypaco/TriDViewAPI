using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TriDViewAPI.Configurations;
using TriDViewAPI.Data;
using TriDViewAPI.Extensions;
using TriDViewAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DbConnection"]));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddOptions<AppSettings>()
    .Bind(builder.Configuration.GetSection("AppSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddApplicationServices();
builder.Services.AddApplicationRepositories();

builder.Services.AddCors(p => p.AddPolicy("AllowAnyOrigin",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

builder.Configuration.AddJsonFile("appsettings.Development.json");
builder.Services.AddJwtAuthentication(builder.Configuration);
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
