using System.Reflection;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace LocalizeUs.Patches;

[HarmonyPatch]
public static class TmpAwakePatch
{
    public static TMP_FontAsset LibSansRegTmp;
    public static TMP_FontAsset VcrRegTmp;
    public static TMP_FontAsset BarlowRegTmp;
    public static TMP_FontAsset BarlowBoldTmp;
    public static TMP_FontAsset BarlowSemiBoldTmp;
    public static TMP_FontAsset BarlowBoldItalicTmp;
    private static bool _fallbackRegistered;

    [HarmonyPatch(typeof(TextMeshPro), nameof(TextMeshPro.Awake))]
    [HarmonyPostfix]
    public static void TmpAwakePostfix(TextMeshPro __instance)
    {
        if (!LibSansRegTmp)
        {
            LibSansRegTmp = LoadFontFromResources("LocalizeUs.Resources.LiberationSans.ttf")!;
        }
        if (!VcrRegTmp)
        {
            VcrRegTmp = LoadFontFromResources("LocalizeUs.Resources.vcr-osd-replayed.ttf")!;
        }
        if (!BarlowRegTmp)
        {
            BarlowRegTmp = LoadFontFromResources("LocalizeUs.Resources.Barlow-Regular.ttf")!;
        }
        if (!BarlowSemiBoldTmp)
        {
            BarlowSemiBoldTmp = LoadFontFromResources("LocalizeUs.Resources.Barlow-SemiBold.ttf")!;
        }
        if (!BarlowBoldItalicTmp)
        {
            BarlowBoldItalicTmp = LoadFontFromResources("LocalizeUs.Resources.Barlow-BoldItalic.ttf")!;
        }
        if (!BarlowBoldTmp)
        {
            BarlowBoldTmp = LoadFontFromResources("LocalizeUs.Resources.Barlow-Bold.ttf")!;
        }
        if (__instance.font.name == "LiberationSans SDF")
        {
            // Instead of replacing the font entirely (which causes rendering
            // differences in weight, outline, and breaks other asset-bundle
            // fonts), register the extended font as a fallback on the original
            // LiberationSans. The original font renders Latin text exactly as
            // the base game does; the extended font only kicks in for glyphs
            // that are missing from the original (e.g. ĄČĘĖĮŠŲŪŽ).
            if (!_fallbackRegistered)
            {
                RegisterFallback(__instance.font, LibSansRegTmp);
                LibSansRegTmp.fontWeightTable = __instance.font.fontWeightTable;
                _fallbackRegistered = true;
            }
        }
        else if (__instance.font.name == "VCR SDF")
        {
            __instance.font = VcrRegTmp;
        }
        else if (__instance.font.name == "Barlow-BoldItalic Masked" || __instance.font.name == "Barlow-BoldItalic SDF")
        {
            __instance.font = BarlowBoldItalicTmp;
        }
        else if (__instance.font.name == "Barlow-SemiBold Masked" || __instance.font.name == "Barlow-SemiBold SDF")
        {
            __instance.font = BarlowSemiBoldTmp;
        }
        else if (__instance.font.name == "Barlow-Regular Masked" || __instance.font.name == "Barlow-Regular SDF")
        {
            __instance.font = BarlowRegTmp;
        }
        else if (__instance.font.name == "Barlow-Bold Masked" || __instance.font.name == "Barlow-Bold SDF")
        {
            __instance.font = BarlowBoldTmp;
        }
        /*else if (component.font.name == "Brook SDF" && CustomLocale.LangsWithCustomFont.Contains(auLang))
        {
            ogBrookTmp = component.font;
            component.font = LocaleUsAssets.AmaticScBoldTmp;
        }*/
    }

    private static void RegisterFallback(TMP_FontAsset mainFont, TMP_FontAsset fallbackFont)
    {
        var fallbacks = mainFont.fallbackFontAssetTable;

        // Check whether the fallback is already registered.
        if (fallbacks != null)
        {
            foreach (var f in fallbacks)
            {
                if (f == fallbackFont)
                    return;
            }
        }

        // Create or extend the fallback list.
        var newList = new Il2CppSystem.Collections.Generic.List<TMP_FontAsset>();
        if (fallbacks != null)
        {
            foreach (var f in fallbacks)
                newList.Add(f);
        }
        newList.Add(fallbackFont);
        mainFont.fallbackFontAssetTable = newList;
    }
    internal static TMP_FontAsset? LoadFontFromResources(string resourcePath)
    {
        try
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using Stream? stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                Error($"Font resource not found: {resourcePath}");
                return null;
            }

            string tempFileName = $"{Path.GetFileNameWithoutExtension(resourcePath)}_{Guid.NewGuid()}{Path.GetExtension(resourcePath)}";
            string tempPath = Path.Combine(Application.temporaryCachePath, tempFileName);

            using (FileStream fileStream = File.Create(tempPath))
            {
                stream.CopyTo(fileStream);
            }

            Font newFont = new(tempPath);
            TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(newFont);
            File.Delete(tempPath);

            return fontAsset;
        }
        catch (Exception ex)
        {
            Error(ex);
            return null;
        }
    }
}