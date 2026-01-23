using System.Globalization;
using System.Text;
using AmongUs.Data;
using AmongUs.Data.Settings;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace LocalizeUs.Patches;

[HarmonyPatch]
public static class TranslatorTmpPatch
{
    /*public static Font ogBrookFont;
    public static TMP_FontAsset ogBrookTmp;*/
    [HarmonyPatch(typeof(TextTranslatorTMP), nameof(TextTranslatorTMP.ResetText))]
    [HarmonyPrefix]
    public static void CurrentLanguagePostfix(TextTranslatorTMP __instance)
    {
        var auLang = (ExtendedLangs)(TranslationController.InstanceExists
            ? TranslationController.Instance.currentLanguage.languageID
            : SupportedLangs.English);
        TextMeshPro component = __instance.GetComponent<TextMeshPro>();
        if (component != null)
        {
            if (component.font.name == "LiberationSans SDF")
            {
                component.font = LocaleUsAssets.LibSansRegTmp;
            }
            /*else if (component.font.name == "Brook SDF" && CustomLocale.LangsWithCustomFont.Contains(auLang))
            {
                ogBrookTmp = component.font;
                component.font = LocaleUsAssets.AmaticScBoldTmp;
            }
            else if (component.font.name == "AmaticSC-Bold" && !CustomLocale.LangsWithCustomFont.Contains(auLang))
            {
                component.font = ogBrookTmp;
            }*/
            component.ForceMeshUpdate(false, false);
        }
        else
        {
            TextMeshProUGUI component2 = __instance.GetComponent<TextMeshProUGUI>();
            if (component2.font.name == "LiberationSans SDF")
            {
                component2.font = LocaleUsAssets.LibSansRegTmp;
            }
            /*else if (component2.font.name == "Brook" && CustomLocale.LangsWithCustomFont.Contains(auLang))
            {
                ogBrookTmp = component2.font;
                component2.font = LocaleUsAssets.AmaticScBoldTmp;
            }
            else if (component2.font.name == "AmaticSC-Bold" && !CustomLocale.LangsWithCustomFont.Contains(auLang))
            {
                component2.font = ogBrookTmp;
            }*/
            component2.ForceMeshUpdate(false, false);
        }
    }

    [HarmonyPatch(typeof(TextMeshPro), nameof(TextMeshPro.Awake))]
    [HarmonyPrefix]
    public static void CurrentLanguagePostfix(TextMeshPro __instance)
    {
        if (__instance.font.name == "LiberationSans SDF")
        {
            __instance.font = LocaleUsAssets.LibSansRegTmp;
        }

        /*else if (component.font.name == "Brook SDF" && CustomLocale.LangsWithCustomFont.Contains(auLang))
        {
            ogBrookTmp = component.font;
            component.font = LocaleUsAssets.AmaticScBoldTmp;
        }
        else if (component.font.name == "AmaticSC-Bold" && !CustomLocale.LangsWithCustomFont.Contains(auLang))
        {
            component.font = ogBrookTmp;
        }*/
        __instance.ForceMeshUpdate(false, false);
    }
}