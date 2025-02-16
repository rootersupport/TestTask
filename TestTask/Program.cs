using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Repositories;
using TestTask.Services;

var builder = WebApplication.CreateBuilder(args);

// Рег контроллеров
builder.Services.AddControllers(); 

// Рег контекста
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Рег репозиториев и сервисов
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();