using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using Microsoft.Extensions.Configuration;
using WebApi.Models;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using WebApi.Repo;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Microsoft.IdentityModel.Logging;
using System.Xml.Linq;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Server=(localdb)\\mssqllocaldb;Database=React+Dot;Trusted_Connection=True;";


// Add services to the container.
builder.Services.AddDbContext<Context>(opt => { opt.UseSqlServer(connectionString);opt.EnableSensitiveDataLogging();}) ;

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("Development")}.json", optional: true);
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<Repository<User>>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("OnlyForOwner", policy =>
    {
        policy.RequireClaim("role", "owner");

    });

    opts.AddPolicy("OnlyForAdmin", policy =>
    {
        policy.RequireClaim("role", "admin");
    });

    opts.AddPolicy("OnlyForTechnician", policy =>
    {
        policy.RequireClaim("role", "technician");
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,

        };
    });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/login";
    });

builder.Services.AddCors(c =>
{
    c.AddPolicy("AlloweOrigin", opt => opt.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader());
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/GetAllTechnicians", [Authorize(Policy = "OnlyForTechnician")] async ( Context context) =>


    Results.Ok(await context.Users!.ToListAsync()));

app.Logger.LogInformation("starting the app ...");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
