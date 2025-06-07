using CinemaReservationApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization; // <-- to dodaj

var builder = WebApplication.CreateBuilder(args);

// Dodaj CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5130") // frontend port
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Po³¹czenie z baz¹ danych
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Kontrolery i Swagger
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // <--- to dodaj

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger tylko w trybie developerskim
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// W³¹cz CORS tu¿ przed autoryzacj¹
app.UseCors();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
