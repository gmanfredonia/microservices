using Admin.API.Extensions;
using Admin.Domain.Contracts.Security;
using Admin.Persistence.Database;
using Admin.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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


