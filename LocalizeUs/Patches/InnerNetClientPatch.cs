using AmongUs.Data;
using HarmonyLib;
using InnerNet;

namespace LocalizeUs.Patches;


[HarmonyPatch]
public static class InnerNetClientPatch
{
    [HarmonyPatch(typeof(InnerNetClient), nameof(InnerNetClient.GetConnectionData))]
    [HarmonyPrefix]
    public static void GetConnectionDataPrefix()
    {
        var lang = DataManager.Settings.Language.CurrentLanguage;
        if ((int)lang >= 16)
        {
            DataManager.Settings.Language.CurrentLanguage = SupportedLangs.English;
        }
    }

    [HarmonyPatch(typeof(InnerNetClient), nameof(InnerNetClient.GetConnectionData))]
    [HarmonyPostfix]
    public static void GetConnectionDataPostfix()
    {
        if (TranslationController.InstanceExists)
        {
            var realLang = TranslationController.Instance.currentLanguage.languageID;
            if (DataManager.Settings.Language.CurrentLanguage != realLang)
            {
                DataManager.Settings.Language.CurrentLanguage = realLang;
            }
        }
    }
}
