using Admin.API.Extensions;
using Admin.Domain.Contracts.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Products.Persistence.Database;
using Products.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomExceptionHandler();
builder.Services.AddCustomLocalization(builder.Configuration);
builder.Services.AddCustomValidation();
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

app.MapGet("/all", async (IServiceManager serviceManager) =>
    Results.Ok(await serviceManager.ServiceProducts.ProductsGetAllAsync().ConfigureAwait(false)))
   .Produces<IEnumerable<DTOProductRow>>(StatusCodes.Status200OK)
   .Produces(StatusCodes.Status401Unauthorized)
   .Produces(StatusCodes.Status500InternalServerError)
   .RequireAuthorization()
   .WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
