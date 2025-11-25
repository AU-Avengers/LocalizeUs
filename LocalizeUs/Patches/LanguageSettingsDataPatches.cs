using AmongUs.Data.Settings;
using HarmonyLib;

namespace LocalizeUs.Patches;

[HarmonyPatch]
public static class LanguageSettingsDataPatches
{
    [HarmonyPatch(typeof(LanguageSettingsData), nameof(LanguageSettingsData.CurrentLanguage), MethodType.Setter)]
    [HarmonyPostfix]
    public static void CurrentLanguagePostfix(LanguageSettingsData __instance)
    {
        var curLang = __instance.CurrentLanguage;
        var text = (ExtendedLangs)curLang;
        if (__instance.language != text.ToString())
        {
            __instance.language = text.ToString();
            __instance.HandleChange();
        }

        __instance.cachedLanguage = curLang;
    }

    [HarmonyPatch(typeof(LanguageSettingsData), nameof(LanguageSettingsData.OnSaveStart))]
    [HarmonyPostfix]
    public static void OnSaveStartPostfix(LanguageSettingsData __instance)
    {
        var customLang = (ExtendedLangs)__instance.cachedLanguage;
        if (Enum.IsDefined(typeof(ExtendedLangs), customLang))
        {
            __instance.language = __instance.cachedLanguage.ToString();
        }
    }
}