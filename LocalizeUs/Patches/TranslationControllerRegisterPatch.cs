using System.Globalization;
using System.Text;
using AmongUs.Data;
using HarmonyLib;
using UnityEngine;

namespace LocalizeUs.Patches;

[HarmonyPatch]
public static class TranslationControllerRegisterPatch
{
    public static void AddLangSet(ExtendedLangs newLanguage, string localName, string engName)
    {
        var dataManager = ReferenceDataManager.Instance.Refdata.languages;
        var englishData = dataManager.ToArray().First(x => x.languageID is SupportedLangs.English);
        var turkish = ScriptableObject.CreateInstance<TranslatedImageSet>();
        turkish.Name = localName;
        turkish.languageID = (SupportedLangs)newLanguage;
        var builder = new StringBuilder();
        // Using "\t" forces a tab space to be placed, which is what the text asset requires to be read by the game.
        builder.Append(CultureInfo.InvariantCulture, $"KEY ID\t{engName}");
        foreach (var pair in CustomLocale.InternalLocalization[newLanguage])
        {
            builder.Append(CultureInfo.InvariantCulture, $"\n{pair.Key}\t{pair.Value}");
        }

        var newLocaleData = new TextAsset(builder.ToString());
        turkish.Data = newLocaleData;
        turkish.CensorSet = englishData.CensorSet;
        dataManager.Add(turkish);
    }

    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.Initialize))]
    [HarmonyPrefix]
    public static void InitPrefix(TranslationController __instance)
    {
        AddLangSet(ExtendedLangs.Polish, "Polski", "Polish");
        AddLangSet(ExtendedLangs.Turkish, "Türkçe", "Turkish");
        AddLangSet(ExtendedLangs.Swedish, "Svenska", "Swedish");
        AddLangSet(ExtendedLangs.Lithuanian, "Lietuvių", "Lithuanian");
    }

[HarmonyPatch(typeof(TranslationController), nameof(TranslationController.SetLanguage))]
    [HarmonyPrefix]
    public static bool SetLanguagePrefix(TranslationController __instance, SupportedLangs language)
    {
        var langNum = (int)language;
        var properLang = (ExtendedLangs)langNum;
        if (!Enum.IsDefined(properLang))
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