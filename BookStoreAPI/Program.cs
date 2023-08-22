using System.Text.Json.Serialization;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using BookStoreAPI.Models.Repositories;
using BookStoreAPI.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(folder);
var DBPath = Path.Join(path, "bookStore.db");
builder.Services.AddDbContext<BookStoreDatabaseContext>(options =>
    options.UseSqlite($"Data Source={DBPath}"));

builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception != null)
        {
            context.Response.ContentType = "application/json";
            if (exception.Error is ApiException apiException)
            {
                context.Response.StatusCode = apiException.StatusCode;
                await context.Response.WriteAsJsonAsync(new
                {
                    apiException.Content
                });
            }
            else
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    exception.Error.Message
                });
            }
        }
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program
{
}