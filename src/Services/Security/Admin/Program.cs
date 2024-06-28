using Admin.Extensions;
using Domain.Contracts.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.Security;
using Persistence.Database.RateIt;
using Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomExceptionHandler();
builder.Services.AddCustomLocalization(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomSwagger();
builder.Services.AddCustomMapster();
builder.Services.AddCustomServices();

builder.Services.AddAuthorization();
builder.Services.AddCors();
builder.Services.AddDbContextPool<DbContextRateIt>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("rateItConnection"))
                                                                     .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                                                     .LogTo(Console.WriteLine));
builder.Logging.AddCustomLogging(builder.Configuration);


var app = builder.Build();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseSwagger();
if (app.Environment.IsDevelopment())
    app.UseSwaggerUI();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});
app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.RequireAuthorization()
.WithOpenApi();

app.MapGet("/all", async (IServiceManager serviceManager) =>
    Results.Ok(await serviceManager.ServiceProducts.ProductsGetAllAsync().ConfigureAwait(false)))
   .Produces<IEnumerable<DTOProductRow>>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status401Unauthorized)
   .Produces(StatusCodes.Status500InternalServerError)
   .RequireAuthorization()
   .WithOpenApi();

app.MapPost("/createToken", async (IServiceManager serviceManager, DTOLogin user) =>
{
    IResult response;
    DTOToken result;

    result = await serviceManager.ServiceSecurity.CreateTokenAsync(user).ConfigureAwait(false);
    response = Results.Ok(result);

    return response;
})
.Produces<IEnumerable<DTOToken>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.AllowAnonymous()
.WithOpenApi();

app.MapGet("/getCurrentEnvironment", (IServiceManager serviceManager, IWebHostEnvironment environment) =>
{
    IResult response;
    string result;

    //throw new ApplicationException("Si è verificato un grave errore!");
    //await Task.Delay(1000 * 5);

    result = environment.EnvironmentName;
    response = Results.Ok(result);

    return response;
})
.Produces<IEnumerable<DTOToken>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status500InternalServerError)
.AllowAnonymous()
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();

//app.Run();

//internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}


//internal sealed class GlobalExceptionHandler : IExceptionHandler
//{
//    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
//    {
//        throw new NotImplementedException();
//    }
//}