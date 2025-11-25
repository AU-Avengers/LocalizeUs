/*using AmongUs.Data.Settings;
using HarmonyLib;

namespace LocalizeUs.Patches;

[HarmonyPatch]
public static class ChatLanguageSetPatch
{
    [HarmonyPatch(typeof(ChatLanguageSet), nameof(ChatLanguageSet.Languages), MethodType.Getter)]
    [HarmonyPrefix]
    public static bool LanguagePrefix(ref Dictionary<string, uint> __result)
    {
        var newDict = new Dictionary<string, uint>()
        {
            {
                "English",
                256U
            },
            {
                "Español (Latam)",
                2U
            },
            {
                "Português (BR)",
                2048U
            },
            {
                "Português",
                16U
            },
            {
                "한국어",
                4U
            },
            {
                "Pусский",
                8U
            },
            {
                "Nederlands",
                4096U
            },
            {
                "Bisaya",
                64U
            },
            {
                "Français",
                8192U
            },
            {
                "Deutsch",
                16384U
            },
            {
                "Italiano",
                32768U
            },
            {
                "日本語",
                512U
            },
            {
                "Español",
                1024U
            },
            {
                "Al Arabiya",
                32U
            },
            {
                "Polski",
                128U
            },
            {
                "简体中文",
                65536U
            },
            {
                "繁體中文",
                131072U
            },
            {
                "Gaeilge",
                262144U
            },
            {
                "Other",
                1U
            }
        }
        return false;
    }
}*/