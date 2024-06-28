using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Localization;

public static class LocalizationExtensions
{
    public static void AddLocalization(this IServiceCollection services)
    {
        LocalizationServiceCollectionExtensions.AddLocalization(services);        
        services.AddSingleton<IStringLocalizer, StringLocalizer>();
    }
    public  static void AddLanguage(this IServiceCollection services, Action<CultureMode> languageOptions)
    {
        //For OptionsMonitor see https://stackoverflow.com/questions/63585101/why-is-ioptionsmonitort-onchange-not-being-called
        services.Configure(languageOptions);
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        
        services.Configure<RequestLocalizationOptions>(options =>
        {
            IList<CultureInfo> supportedCultures = [new("en-US"), new("en"), new("it-IT"), new("it")];

            options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
            options.SupportedCultures = options.SupportedUICultures = supportedCultures;

            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(ActivatorUtilities.CreateInstance<CultureProvider>(serviceProvider, options));
        });
    }
}