using Microsoft.EntityFrameworkCore;
using TechLibrary.Api.Filters;
using TechLibrary.Application.UseCases.Users.Login.DoLogin;
using TechLibrary.Application.UseCases.Users.Register;
using TechLibrary.Infrastructure.Data;
using TechLibrary.Infrastructure.Security.Cryptography;
using TechLibrary.Persistence.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TechLibraryDbContext>(options =>
    options.UseSqlite(connectionString)
);

builder.Services.AddScoped<ITechLibraryDbContext, TechLibraryDbContext>();
builder.Services.AddScoped<RegisterUserUsecase>();
builder.Services.AddScoped<DoLoginUseCase>();
builder.Services.AddScoped<RegisterUserValidator>();
builder.Services.AddScoped<BCryptAlgorithm>();

builder.Services.AddMvc(option => option.Filters.Add(new ExceptionFilter()));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TechLibraryDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();