using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using BepInEx.Logging;
using Reactor.Localization;
using UnityEngine;

namespace LocalizeUs.Localization;

public static class CustomLocale
{
    public static string LocaleDirectory => Path.Combine(Application.persistentDataPath, "LocalizeUs", "Locales");

    public static Dictionary<ExtendedLangs, string> LangList { get; } = new()
    {
        { ExtendedLangs.English, "en_US.xml" },
        { ExtendedLangs.Latam, "es_419.xml" },
        { ExtendedLangs.Brazilian, "pt_BR.xml" },
        { ExtendedLangs.Portuguese, "pt_PT.xml" },
        { ExtendedLangs.Korean, "ko_KR.xml" },
        { ExtendedLangs.Russian, "ru_RU.xml" },
        { ExtendedLangs.Dutch, "nl_NL.xml" },
        { ExtendedLangs.Filipino, "fil_PH.xml" },
        { ExtendedLangs.French, "fr_FR.xml" },
        { ExtendedLangs.German, "de_DE.xml" },
        { ExtendedLangs.Italian, "it_IT.xml" },
        { ExtendedLangs.Japanese, "ja_JP.xml" },
        { ExtendedLangs.Spanish, "es_ES.xml" },
        { ExtendedLangs.SimplifiedChinese, "zh_CN.xml" },
        { ExtendedLangs.TraditionalChinese, "zh_TW.xml" },
        { ExtendedLangs.Irish, "ga_IE.xml" },
        { ExtendedLangs.Polish, "pl_PL.xml" }, // Custom
        { ExtendedLangs.Turkish, "tr_TR.xml" }, // Custom
        { ExtendedLangs.Swedish, "sv_SE.xml" }, // Custom
        { ExtendedLangs.Lithuanian, "lt_LT.xml" }, // Custom
        { ExtendedLangs.Czech, "cs_CZ.xml" }, // Custom
        { ExtendedLangs.LiteraryChinese, "lzh_CN.xml" }, // Custom
        { ExtendedLangs.Greek, "el-GR.xml" }, // Custom
    };
    public static Dictionary<ExtendedLangs, string> LangListExternal { get; } = new()
    {
        { ExtendedLangs.English, "LU_en_US.xml" },
        { ExtendedLangs.Latam, "LU_es_419.xml" },
        { ExtendedLangs.Brazilian, "LU_pt_BR.xml" },
        { ExtendedLangs.Portuguese, "LU_pt_PT.xml" },
        { ExtendedLangs.Korean, "LU_ko_KR.xml" },
        { ExtendedLangs.Russian, "LU_ru_RU.xml" },
        { ExtendedLangs.Dutch, "LU_nl_NL.xml" },
        { ExtendedLangs.Filipino, "LU_fil_PH.xml" },
        { ExtendedLangs.French, "LU_fr_FR.xml" },
        { ExtendedLangs.German, "LU_de_DE.xml" },
        { ExtendedLangs.Italian, "LU_it_IT.xml" },
        { ExtendedLangs.Japanese, "LU_ja_JP.xml" },
        { ExtendedLangs.Spanish, "LU_es_ES.xml" },
        { ExtendedLangs.SimplifiedChinese, "LU_zh_CN.xml" },
        { ExtendedLangs.TraditionalChinese, "LU_zh_TW.xml" },
        { ExtendedLangs.Irish, "LU_ga_IE.xml" },
        { ExtendedLangs.Polish, "LU_pl_PL.xml" }, // Custom
        { ExtendedLangs.Turkish, "LU_tr_TR.xml" }, // Custom
        { ExtendedLangs.Swedish, "LU_sv_SE.xml" }, // Custom
        { ExtendedLangs.Lithuanian, "LU_lt_LT.xml" }, // Custom
        { ExtendedLangs.Czech, "LU_cs_CZ.xml" }, // Custom
        { ExtendedLangs.LiteraryChinese, "LU_lzh_CN.xml" }, // Custom
        { ExtendedLangs.Greek, "LU_el-GR.xml" }, // Custom
    };
    public static Dictionary<ExtendedLangs, string> LangCultureList { get; } = new()
    {
        { ExtendedLangs.English, "en-US" },
        { ExtendedLangs.Latam, "es-419" },
        { ExtendedLangs.Brazilian, "pt-BR" },
        { ExtendedLangs.Portuguese, "pt-PT" },
        { ExtendedLangs.Korean, "ko-KR" },
        { ExtendedLangs.Russian, "ru-RU" },
        { ExtendedLangs.Dutch, "nl-NL" },
        { ExtendedLangs.Filipino, "fil-PH" },
        { ExtendedLangs.French, "fr-FR" },
        { ExtendedLangs.German, "de-DE" },
        { ExtendedLangs.Italian, "it-IT" },
        { ExtendedLangs.Japanese, "ja-JP" },
        { ExtendedLangs.Spanish, "es-ES" },
        { ExtendedLangs.SimplifiedChinese, "zh-CN" },
        { ExtendedLangs.TraditionalChinese, "zh-TW" },
        { ExtendedLangs.Irish, "ga-IE" },
        { ExtendedLangs.Polish, "pl-PL" }, // Custom
        { ExtendedLangs.Turkish, "tr-TR" }, // Custom
        { ExtendedLangs.Swedish, "sv-SE" }, // Custom
        { ExtendedLangs.Lithuanian, "lt-LT" }, // Custom
        { ExtendedLangs.Czech, "cs-CZ" }, // Custom
        { ExtendedLangs.LiteraryChinese, "zh-TW" }, // Custom
        { ExtendedLangs.Greek, "el-GR" }, // Custom
    };
    public static Dictionary<ExtendedLangs, string> LangCodesList { get; } = new()
    {
        { ExtendedLangs.English, "en" },
        { ExtendedLangs.Latam, "es" },
        { ExtendedLangs.Brazilian, "pt" },
        { ExtendedLangs.Portuguese, "pt" },
        { ExtendedLangs.Korean, "ko" },
        { ExtendedLangs.Russian, "ru" },
        { ExtendedLangs.Dutch, "nl" },
        { ExtendedLangs.Filipino, "fil" },
        { ExtendedLangs.French, "fr" },
        { ExtendedLangs.German, "de" },
        { ExtendedLangs.Italian, "it" },
        { ExtendedLangs.Japanese, "ja" },
        { ExtendedLangs.Spanish, "es" },
        { ExtendedLangs.SimplifiedChinese, "zh" },
        { ExtendedLangs.TraditionalChinese, "zh" },
        { ExtendedLangs.Irish, "ga" },
        { ExtendedLangs.Polish, "pl" }, // Custom
        { ExtendedLangs.Turkish, "tr" }, // Custom
        { ExtendedLangs.Swedish, "sv" }, // Custom
        { ExtendedLangs.Lithuanian, "lt" }, // Custom
        { ExtendedLangs.Czech, "cs" }, // Custom
        { ExtendedLangs.LiteraryChinese, "lzh" }, // Custom
        { ExtendedLangs.Greek, "el" }, // Custom
    };

    public static string BepinexLocaleDirectory =>
        Path.Combine(BepInEx.Paths.BepInExRootPath, "CustomLocales", "LocalizeUs");

    public static Dictionary<string, string> TmpTextList { get; } = new()
    {
        { "<nl>", "\n" },
        { "<and>", "&" },
    };

    public static Dictionary<ExtendedLangs, Dictionary<string, string>> InternalLocalization { get; } = [];
    public static Dictionary<ExtendedLangs, Dictionary<string, string>> CustomLocalization { get; } = [];

    internal static ManualLogSource Logger { get; } = BepInEx.Logging.Logger.CreateLogSource("CustomLocale");

    public static string Get(string name, string? defaultValue = null)
    {
        var currentLanguage =
            TranslationController.InstanceExists
                ? TranslationController.Instance.currentLanguage.languageID
                : SupportedLangs.English;
        return Get(HelperUtils.ToCustom(currentLanguage), name, defaultValue);
    }

    public static string Get(ExtendedLangs language, string name, string? defaultValue = null)
    {
        if (CustomLocalization.TryGetValue(language, out var translations) &&
            translations.TryGetValue(name, out var translation))
        {
            return translation;
        }

        if (CustomLocalization.TryGetValue(ExtendedLangs.English, out var translationsEng) &&
            translationsEng.TryGetValue(name, out var translationEng))
        {
            return translationEng;
        }

        return defaultValue ?? "STRMISS_" + name;
    }
    public static string GetParsed(string name, string? defaultValue = null,
        Dictionary<string, string>? parseList = null)
    {
        var currentLanguage =
            TranslationController.InstanceExists
                ? TranslationController.Instance.currentLanguage.languageID
                : SupportedLangs.English;
        return GetParsed(HelperUtils.ToCustom(currentLanguage), name, defaultValue, parseList);
    }

    public static string GetParsed(ExtendedLangs language, string name, string? defaultValue = null,
        Dictionary<string, string>? parseList = null)
    {
        var text = defaultValue ?? "STRMISS_" + name;

        if (CustomLocalization.TryGetValue(ExtendedLangs.English, out var translationsEng) &&
            translationsEng.TryGetValue(name, out var translationEng))
        {
            text = translationEng;
        }

        if (language is not ExtendedLangs.English && CustomLocalization.TryGetValue(language, out var translations) &&
            translations.TryGetValue(name, out var translation))
        {
            text = translation;
        }

        foreach (var tmpText in TmpTextList.Where(x => text.Contains(x.Key)))
        {
            text = text.Replace(tmpText.Key, tmpText.Value);
        }

        if (parseList != null)
        {
            foreach (var tmpText in parseList.Where(x => text.Contains(x.Key)))
            {
                text = text.Replace(tmpText.Key, tmpText.Value);
            }
        }

        return text;
    }

    public static void Initialize()
    {
        LocalizationManager.Register(new CustomLocalizationProvider());
        SearchInternalLocale();
    }

    public static void LoadExternalLocale()
    {
        SearchDirectory(BepInEx.Paths.PluginPath);
        SearchDirectory(BepInEx.Paths.BepInExRootPath);
        SearchDirectory(BepinexLocaleDirectory);
        SearchDirectory(BepInEx.Paths.GameRootPath);
        SearchDirectory(LocaleDirectory);
    }

    public static void SearchInternalLocale()
    {
        var assembly = Assembly.GetExecutingAssembly();
        foreach (var locale in LangList)
        {
            using var resourceStream =
                assembly.GetManifestResourceStream("LocalizeUs.Resources.Locale." + locale.Value);
            if (resourceStream == null)
            {
                Logger.LogError($"Language is not added: {locale.Key.ToDisplayString()}");
                continue;
            }

            Logger.LogWarning($"Language is being added: {locale.Key.ToDisplayString()}");
            using StreamReader reader = new(resourceStream);
            string xmlContent = reader.ReadToEnd();

            InternalLocalization.TryAdd(locale.Key, []);
            ParseXmlFile(xmlContent, locale.Key, true);
        }
    }

    public static void SearchDirectory(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Logger.LogError($"Directory does not exist: {directory}");
            return;
        }

        var xmlTranslations = Directory.GetFiles(directory, "*.xml");
        foreach (var file in xmlTranslations)
        {
            var localeName = Path.GetFileNameWithoutExtension(file);
            if (!LangListExternal.ContainsValue(localeName + ".xml"))
            {
                Logger.LogError($"Invalid locale iso name: {localeName}");
                continue;
            }

            Logger.LogWarning($"Adding locale for: {localeName} in {file}");

            var language = LangListExternal.FirstOrDefault(x => x.Value == localeName + ".xml").Key;
            CustomLocalization.TryAdd(language, []);
            var xmlContent = File.ReadAllText(file);
            ParseXmlFile(xmlContent, language, false);
        }
    }

    public static void ParseXmlFile(string xmlContent, ExtendedLangs language, bool isInternal)
    {
        var localeList = isInternal ? InternalLocalization : CustomLocalization;
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.LoadXml(xmlContent);
            XmlNodeList? stringNodes = xmlDoc.SelectNodes("/resources/string");

            if (stringNodes != null)
            {
                Logger.LogWarning($"{stringNodes.Count} XML Nodes found!");
                foreach (XmlNode node in stringNodes)
                {
                    if (node.Attributes?["name"] != null)
                    {
                        string name = node.Attributes["name"]!.Value;
                        string value = node.InnerText;
                        
                        if (localeList[language].ContainsKey(name))
                        {
                            var ogValuePair = localeList[language].FirstOrDefault(x => x.Key == name);
                            localeList[language].Remove(ogValuePair.Key);
                        }

                        if (value.Contains("\\%"))
                        {
                            value = Regex.Replace(value, @"\%([^%]+)\%", @"<$1>");
                            if (value.Contains("\\<"))
                            {
                                value = value.Replace("\\<", "<");
                            }

                            if (value.Contains("\\>"))
                            {
                                value = value.Replace("\\>", ">");
                            }
                        }

                        localeList[language].TryAdd(name, value);
                    }
                }

                Logger.LogWarning(
                    $"{localeList[language].Count} Localization strings added to {language.ToDisplayString()}!");
            }
            else
            {
                Logger.LogError($"XML nodes were not found in {xmlContent}.");
            }
        }
        catch (XmlException ex)
        {
            Logger.LogError($"XML parsing error: {ex.Message}");
        }
    }
}