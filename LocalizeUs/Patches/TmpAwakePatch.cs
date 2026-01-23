using HarmonyLib;
using TMPro;

namespace LocalizeUs.Patches;

[HarmonyPatch]
public static class TmpAwakePatch
{
    [HarmonyPatch(typeof(TextMeshPro), nameof(TextMeshPro.Awake))]
    [HarmonyPostfix]
    public static void TmpAwakePostfix(TextMeshPro __instance)
    {
        if (__instance.font.name == "LiberationSans SDF")
        {
            __instance.font = LocaleUsAssets.LibSansRegTmp;
        }
        /*else if (component.font.name == "Brook SDF" && CustomLocale.LangsWithCustomFont.Contains(auLang))
        {
            ogBrookTmp = component.font;
            component.font = LocaleUsAssets.AmaticScBoldTmp;
        }*/
        __instance.ForceMeshUpdate(false, false);
    }
}