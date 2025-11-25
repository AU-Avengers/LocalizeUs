using System.Linq;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Reactor.Localization;
using Reactor.Localization.Providers;
using Reactor.Utilities;

namespace LocalizeUs.Localization;

public class CustomLocalizationProvider : LocalizationProvider
{
    private static bool _loadedStrings;
    public override int Priority => ReactorPriority.High;

    public override bool TryGetText(StringNames stringName, out string? result)
    {
        if ((int)stringName >= 0)
        {
            var localeText = CustomLocale.GetParsed(stringName);
            if (!localeText.Contains("STRMISS"))
            {
                result = localeText;
                return true;
            }
        }
        result = null;
        return false;
    }

    public override bool TryGetTextFormatted(StringNames stringName, Il2CppReferenceArray<Il2CppSystem.Object> parts, out string? result)
    {
        if (!TryGetText(stringName, out result)) return false;

        result = Il2CppSystem.String.Format(result, parts);
        return true;
    }

    public override void OnLanguageChanged(SupportedLangs newLanguage)
    {
        if (!_loadedStrings)
        {
            CustomLocale.LoadExternalLocale();
            _loadedStrings = true;
        }

        if (CustomLocale.LangCultureList.TryGetValue(HelperUtils.ToCustom(newLanguage), out var culture))
        {
            LocalizeUsPlugin.Culture = new(culture);
        }
        /*Warning($"<?xml version='1.0' encoding='UTF-8'?>");
        Warning($"<resources>");
        foreach (var stringName in TranslationController.Instance.currentLanguage.AllStrings)
        {
            var value = stringName.Value.Replace("\n", "\\%nl\\%");
            value = value.Replace("&", "\\%and\\%");
            value = value.Replace("<", "\\%");
            value = value.Replace(">", "\\%");
            Warning($"<string name=\"{stringName.Key}\">{value}</string>");
        }
        Warning($"</resources>");*/
    }
}