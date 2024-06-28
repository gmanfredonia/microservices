using Microsoft.Extensions.Localization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Localization;

public class StringLocalizer(IStringLocalizerFactory localizerFactory) : IStringLocalizer
{
    public LocalizedString this[string name]
        => localizer[name];
    public LocalizedString this[string name, params object[] arguments]
        => (arguments.Length > 0) ? localizer[name, arguments] : localizer[name];
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        => localizer.GetAllStrings(includeParentCultures);
    
    private readonly IStringLocalizer localizer = localizerFactory.Create("Localization", "Resources");
}