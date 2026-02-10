
// NEW:
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Soln # 2.1 Move here the AddControllers in the upper section 
// NEW: # 2.1
// Controllers
builder.Services.AddControllers();

// CORS.    Angular, (ASP.NET Core API), 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:4200",    // Localhost NG SERVE
                "http://localhost:88"       // IIS, // Soln # 2.2 Ad the IIS port 
                 )
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Soln # 2.1 To move the controller in the upper section 
// OLD: # 2.1
// builder.Services.AddControllers();

// DbContext (REGISTER ONCE)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// NEW: Swagger services
// Service registration (Dependency Injection)
// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();  // .NET 8 dont have this. only .NET 10 SDK have this

// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Swagger (enable in Azure too)
app.UseSwagger();
app.UseSwaggerUI();

// Pipeline order MATTERS
app.UseHttpsRedirection();  // Typical pipeline

// NEW: Swagger middleware
// These lines add Swagger to the HTTP request pipeline:
if (app.Environment.IsDevelopment())
{
   // How Angular uses Swagger:
   // Manually test API before Angular integration
   // Generate Angular services from Swagger
   // Ensure request/response contracts match

                            // Typical pipeline
    app.UseSwagger();       // Swagger JSON middleware, API metadata (JSON), Shows request/response models
    app.UseSwaggerUI();     // Swagger UI middleware, Browser UI
}

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

app.UseRouting();  // Soln # 2.3 NEW   

app.UseCors("AllowAngular");  // Soln # 2.3  NOTE:  👈 MUST be after UseRouting 
                              // NEW: Angular, UseCors must be BEFORE MapControllers().

// NEW: Swagger middleware
app.UseAuthorization();     // Typical pipeline

app.MapControllers();       // Typical pipeline, Soln # 2.3 NOTE: 👈 MUST be after UseCors


// Fix API root call:  https://localhost:xxxx/
app.MapGet("/", () => "API OK");

app.MapGet("/test", () => "API TEST OK");


app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
