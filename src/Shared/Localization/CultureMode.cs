namespace Localization;

public enum CultureWorkingMode
{
    UniqueCulture = 0,
    DoubleCulture
}
public class CultureMode
{
    public CultureWorkingMode WorkingMode { get; set; }

    public const string SectionLanguage = "Language";
}
