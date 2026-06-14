using System.Reflection;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace LocalizeUs.Patches;

[HarmonyPatch]
public static class TmpAwakePatch
{
    public static TMP_FontAsset LibSansRegTmp;
    public static TMP_FontAsset LibSansBoldTmp;
    public static TMP_FontAsset LibSansItalicTmp;
    public static TMP_FontAsset LibSansBoldItalicTmp;
    public static TMP_FontAsset VcrRegTmp;
    public static TMP_FontAsset BarlowRegTmp;
    public static TMP_FontAsset BarlowBoldTmp;
    public static TMP_FontAsset BarlowSemiBoldTmp;
    public static TMP_FontAsset BarlowBoldItalicTmp;
    public static TMP_FontAsset NotoSansArabicTmp;
    private static bool _fallbackRegistered;

    [HarmonyPatch(typeof(TextMeshPro), nameof(TextMeshPro.Awake))]
    [HarmonyPostfix]
    public static void TmpAwakePostfix(TextMeshPro __instance)
    {
        if (!LibSansRegTmp)
        {
            LibSansRegTmp = LoadFontFromResources("LocalizeUs.Resources.LiberationSans-Regular.ttf")!;
            LibSansRegTmp.name = "LiberationSans Regular (Custom)";
        }
        if (!LibSansBoldTmp)
        {
            LibSansBoldTmp = LoadFontFromResources("LocalizeUs.Resources.LiberationSans-Bold.ttf")!;
            LibSansBoldTmp.name = "LiberationSans Bold (Custom)";
        }
        if (!LibSansItalicTmp)
        {
            LibSansItalicTmp = LoadFontFromResources("LocalizeUs.Resources.LiberationSans-Italic.ttf")!;
            LibSansItalicTmp.name = "LiberationSans Italic (Custom)";
        }
        if (!LibSansBoldItalicTmp)
        {
            LibSansBoldItalicTmp = LoadFontFromResources("LocalizeUs.Resources.LiberationSans-BoldItalic.ttf")!;
            LibSansBoldItalicTmp.name = "LiberationSans Bold Italic (Custom)";
        }
        if (!NotoSansArabicTmp)
        {
            NotoSansArabicTmp = LoadFontFromResources("LocalizeUs.Resources.NotoSans-Arabic.ttf")!;
            NotoSansArabicTmp.name = "NotoSans Arabic (Custom)";
        }
        if (!VcrRegTmp)
        {
            VcrRegTmp = LoadFontFromResources("LocalizeUs.Resources.marisas-vcr-osd-mono-faithful-32x.ttf")!;
            VcrRegTmp.name = "Marisa's VCR OSD Mono 32x (Custom)";
        }
        if (!BarlowRegTmp)
        {
            BarlowRegTmp = LoadFontFromResources("LocalizeUs.Resources.Barlow-Regular.ttf")!;
            BarlowRegTmp.name = "Barlow Regular (Custom)";
        }
        if (!BarlowSemiBoldTmp)
        {
            BarlowSemiBoldTmp = LoadFontFromResources("LocalizeUs.Resources.Barlow-SemiBold.ttf")!;
            BarlowSemiBoldTmp.name = "Barlow Semi Bold (Custom)";
        }
        if (!BarlowBoldItalicTmp)
        {
            BarlowBoldItalicTmp = LoadFontFromResources("LocalizeUs.Resources.Barlow-BoldItalic.ttf")!;
            BarlowBoldItalicTmp.name = "Barlow Bold Italic (Custom)";
        }
        if (!BarlowBoldTmp)
        {
            BarlowBoldTmp = LoadFontFromResources("LocalizeUs.Resources.Barlow-Bold.ttf", 40)!;
            BarlowBoldTmp.name = "Barlow Bold (Custom)";
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
                LibSansRegTmp.fontWeightTable = __instance.font.fontWeightTable;
                var regMat = LibSansRegTmp.material;
                regMat.SetFloat(ShaderUtilities.ID_OutlineWidth, 10f);
                regMat.SetFloat(ShaderUtilities.ID_FaceDilate, 1f);
                RegisterFallback(__instance.font, LibSansRegTmp);
                /*RegisterFallback(__instance.font, LibSansItalicTmp);
                RegisterFallback(__instance.font, LibSansBoldTmp);
                RegisterFallback(__instance.font, LibSansBoldItalicTmp);*/
                RegisterFallback(__instance.font, NotoSansArabicTmp);
                /*LibSansItalicTmp.fontWeightTable = __instance.font.fontWeightTable;
                LibSansBoldTmp.fontWeightTable = __instance.font.fontWeightTable;
                LibSansBoldItalicTmp.fontWeightTable = __instance.font.fontWeightTable;*/
                NotoSansArabicTmp.fontWeightTable = __instance.font.fontWeightTable;
                _fallbackRegistered = true;
                __instance.UpdateMeshPadding();
            }
        }
        else if (__instance.font.name == "VCR SDF")
        {
            RegisterFallback(__instance.font, VcrRegTmp);
            VcrRegTmp.fontWeightTable = __instance.font.fontWeightTable;
        }
        else if (__instance.font.name == "Barlow-BoldItalic Masked" || __instance.font.name == "Barlow-BoldItalic SDF")
        {
            RegisterFallback(__instance.font, BarlowBoldItalicTmp);
            BarlowBoldItalicTmp.fontWeightTable = __instance.font.fontWeightTable;
        }
        else if (__instance.font.name == "Barlow-SemiBold Masked" || __instance.font.name == "Barlow-SemiBold SDF")
        {
            RegisterFallback(__instance.font, BarlowSemiBoldTmp);
            BarlowSemiBoldTmp.fontWeightTable = __instance.font.fontWeightTable;
        }
        else if (__instance.font.name == "Barlow-Regular Masked" || __instance.font.name == "Barlow-Regular SDF")
        {
            RegisterFallback(__instance.font, BarlowRegTmp);
            BarlowRegTmp.fontWeightTable = __instance.font.fontWeightTable;
        }
        else if (__instance.font.name == "Barlow-Bold Masked" || __instance.font.name == "Barlow-Bold SDF")
        {
            RegisterFallback(__instance.font, BarlowBoldTmp);
            BarlowBoldTmp.fontWeightTable = __instance.font.fontWeightTable;
        }
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

    internal static TMP_FontAsset? LoadFontFromResources(string resourcePath, int padding = 18)
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

            string tempFileName =
                $"{Path.GetFileNameWithoutExtension(resourcePath)}_{Guid.NewGuid()}{Path.GetExtension(resourcePath)}";
            string tempPath = Path.Combine(Application.temporaryCachePath, tempFileName);

            using (FileStream fileStream = File.Create(tempPath))
            {
                stream.CopyTo(fileStream);
            }

            Font newFont = new(tempPath);
            TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(newFont,
                90,
                padding,
                UnityEngine.TextCore.LowLevel.GlyphRenderMode.SDFAA,
                2048,
                2048);
            fontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;
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