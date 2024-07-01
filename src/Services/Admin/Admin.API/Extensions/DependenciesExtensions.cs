using Admin.Domain.Contracts;
using Admin.Domain.Repository.Abstractions;
using Admin.Persistence.Database;
using Admin.Persistence.Database.RateIt;
using Admin.Services;
using Admin.Services.Abstractions;
using BuildingBase.Exceptions;
using BuildingBase.Middlewares;
using FluentValidation;
using Localization;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using static DbLogger.DbLoggerExtensions;

namespace Admin.API.Extensions;

public static class DependenciesExtensions
{
    public static void AddCustomExceptionHandler(this IServiceCollection services)
    {
        MappedExceptionCollection mappedExceptions = new();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        mappedExceptions.Map(typeof(DbRowNotFoundException), StatusCodes.Status404NotFound, "messageDbRowNotFound");
        mappedExceptions.Map(typeof(DbUpdateException), StatusCodes.Status409Conflict, "messageDbUpdate");
        mappedExceptions.Map(typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized, "messageUnauthorizedAccess");
        services.AddSingleton<IMappedExceptionCollection>(mappedExceptions);
    }
    public static void AddCustomLocalization(this IServiceCollection services, IConfiguration configuration)
    {
        LocalizationExtensions.AddLocalization(services);
        services.AddLanguage(options => configuration.GetSection(CultureMode.SectionLanguage).Bind(options));
    }
    public static void AddCustomValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<DTOLogin>, DTOLoginValidator>();        
    }
    public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero

            };
            o.Events = new JwtBearerEvents()
            {
                OnChallenge = context => throw new UnauthorizedAccessException()
            };
        });
    }
    public static void AddCustomLogging(this ILoggingBuilder logging, IConfiguration configuration)
    {
        logging.ClearProviders();
        //logging.AddDebug();
        //logging.AddEventSourceLogger();
        //logging.AddEventLog(new Microsoft.Extensions.Logging.EventLog.EventLogSettings() { SourceName = "RateIt" });        
        logging.AddDbLogger(options => configuration.GetSection("Logging:Database:Options").Bind(options));
    }
    public static void AddCustomSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test01", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         Array.Empty<string>()
                    }
            });
        });
    }
    public static void AddCustomMapster(this IServiceCollection services)
    {
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();
        Mappings.Register();
    }
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryReading, RepositoryReading<DbContextRateIt>>();
        services.AddScoped<IRepositoryWriting, RepositoryWriting<DbContextRateIt>>();
        services.AddScoped(provider => new Lazy<IRepositoryReading>(() => provider.GetRequiredService<IRepositoryReading>()));
        services.AddScoped(provider => new Lazy<IRepositoryWriting>(() => provider.GetRequiredService<IRepositoryWriting>()));
        services.AddScoped<IRepositoryManager, RepositoryManager>();

        services.AddScoped<IServiceUsers, ServiceUsers>();        
        services.AddScoped(provider => new Lazy<IServiceUsers>(() => provider.GetRequiredService<IServiceUsers>()));        
        services.AddScoped<IServiceManager, ServiceManager>();
    }
}