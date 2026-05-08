using Customer.Registration.API.Middlewares;
using Customer.Registration.Infrastructure;
using Customer.Registration.Infrastructure.Persistence;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registering the services from the Infrastructure project
builder.Services.AddInfrastructure(builder.Configuration);

// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowRequest",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000/")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// RateLimiter
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100;
    });
});

var app = builder.Build();

// Automatic DB Migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CustomerDBContext>();

    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowRequest");

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
