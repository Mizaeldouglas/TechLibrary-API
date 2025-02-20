using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TechLibrary.Api.Filters;
using TechLibrary.Application.Checkouts;
using TechLibrary.Application.Services.LoggedUser;
using TechLibrary.Application.UseCases.Books;
using TechLibrary.Application.UseCases.Users.Login.DoLogin;
using TechLibrary.Application.UseCases.Users.Register;
using TechLibrary.Infrastructure.Data;
using TechLibrary.Infrastructure.Security.Cryptography;
using TechLibrary.Persistence.Abstractions;

var builder = WebApplication.CreateBuilder(args);
const string AUTHENTICATION_TYPE = "Bearer";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TechLibraryDbContext>(options =>
    options.UseSqlite(connectionString)
);

builder.Services.AddScoped<ITechLibraryDbContext, TechLibraryDbContext>();
builder.Services.AddScoped<LoggedUserService>();

builder.Services.AddScoped<RegisterUserUsecase>();
builder.Services.AddScoped<DoLoginUseCase>();
builder.Services.AddScoped<FilterBookUseCase>();
builder.Services.AddScoped<RegisterBookCheckoutUseCase>();
builder.Services.AddScoped<RegisterUserValidator>();
builder.Services.AddScoped<BCryptAlgorithm>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below;
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = AUTHENTICATION_TYPE
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = AUTHENTICATION_TYPE
                },
                Scheme = "oauth2",
                Name = AUTHENTICATION_TYPE,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


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