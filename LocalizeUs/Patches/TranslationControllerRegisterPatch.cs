using AmongUs.Data;
using AmongUs.Data.Settings;
using HarmonyLib;
using UnityEngine;

namespace LocalizeUs.Patches;

[HarmonyPatch]
public static class TranslationControllerRegisterPatch
{
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.Initialize))]
    [HarmonyPrefix]
    public static void InitPrefix(TranslationController __instance)
    {
        var dataManager = ReferenceDataManager.Instance.Refdata.languages;
        var englishData = dataManager.ToArray().First(x => x.languageID is SupportedLangs.English);
        var polish = ScriptableObject.CreateInstance<TranslatedImageSet>();
        polish.Name = "Polski";
        polish.languageID = (SupportedLangs)ExtendedLangs.Polish;
        polish.Data = englishData.Data;
        polish.CensorSet = englishData.CensorSet;
        dataManager.Add(polish);
    }
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.SetLanguage))]
    [HarmonyPrefix]
    public static bool SetLanguagePrefix(TranslationController __instance, SupportedLangs language)
    {
        var langNum = (int)language;
        var properLang = (ExtendedLangs)langNum;
        if (!Enum.IsDefined(typeof(ExtendedLangs), properLang))
        {
            language = (SupportedLangs)TranslationController.SelectDefaultLanguage();
        }
        Debug(string.Format(LocalizeUsPlugin.Culture, "Setting language to '{0}'", properLang));
        TranslatedImageSet translatedImageSet = __instance.Languages[language];
        DataManager.Settings.Language.CurrentLanguage = translatedImageSet.languageID;
        AnnouncementPopUp.ClearAnnouncementCache();
        __instance.currentLanguage = new LanguageUnit(__instance.Languages[translatedImageSet.languageID]);
        BlockedWords.SetLanguage(translatedImageSet);
        DataManager.Settings.Multiplayer.ValidGameFilterOptions.ResetToDefaults(Il2CppSystem.DateTime.MinValue);
        for (int i = 0; i < __instance.ActiveTexts.Count; i++)
        {
            __instance.ActiveTexts[i].ResetText();
        }

        return false;
    }
}