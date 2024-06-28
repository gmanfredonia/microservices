using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;

namespace Localization;

public class CultureProvider : RequestCultureProvider
{
    public CultureProvider(RequestLocalizationOptions localizationOptions, IOptions<CultureMode> cultureMode)
    {        
        Options = localizationOptions;
        this.cultureMode = cultureMode.Value;

        supportedCultures = Options.SupportedCultures?.Select(c => c.Name) ?? [];
        supportedUICultures = Options.SupportedUICultures?.Select(c => c.Name) ?? [];
    }

    public async override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        ProviderCultureResult result;
        StringWithQualityHeaderValue[] languages;
        string language;
        
        culture = Options.DefaultRequestCulture.Culture.Name;
        uiCulture = Options.DefaultRequestCulture.UICulture.Name;

        if (httpContext.Request.Headers.TryGetValue("Accept-Language", out StringValues values))
        {
            languages = [
                .. values.ToString()
                .Split(',')
                .Select(StringWithQualityHeaderValue.Parse)
                .OrderByDescending(s => s.Quality.GetValueOrDefault(1))
            ];

            if (languages.Length > 0)
            {
                language = languages[0].Value;
                if (cultureMode.WorkingMode == CultureWorkingMode.UniqueCulture)
                {
                    if (supportedCultures.Contains(language, StringComparer.OrdinalIgnoreCase) &&
                        supportedUICultures.Contains(language, StringComparer.OrdinalIgnoreCase))
                        culture = uiCulture = language;
                }
                else
                {
                    language = languages[0].Value;
                    if (supportedCultures.Contains(language, StringComparer.OrdinalIgnoreCase))
                        culture = language;

                    if (languages.Length > 1)
                    {
                        language = languages[1].Value;
                        if (supportedUICultures.Contains(language, StringComparer.OrdinalIgnoreCase))
                            uiCulture = language;
                    }
                }
            }
        }

        result = new ProviderCultureResult(culture, uiCulture);

        return await Task.FromResult(result).ConfigureAwait(false);
    }

    private readonly CultureMode cultureMode;
    private readonly IEnumerable<string> supportedCultures, supportedUICultures;
    private string culture, uiCulture;
}
