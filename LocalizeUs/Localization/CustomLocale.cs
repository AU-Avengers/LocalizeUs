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

    public static List<ExtendedLangs> LangsWithCustomFont { get; } = new()
    {
        ExtendedLangs.Polish, // Custom
        ExtendedLangs.Turkish, // Custom
        ExtendedLangs.Swedish, // Custom
        ExtendedLangs.Lithuanian // Custom
    };
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
        { ExtendedLangs.SChinese, "zh_CN.xml" },
        { ExtendedLangs.TChinese, "zh_TW.xml" },
        { ExtendedLangs.Irish, "ga_IE.xml" },
        { ExtendedLangs.Polish, "pl_PL.xml" }, // Custom
        { ExtendedLangs.Turkish, "tr_TR.xml" }, // Custom
        { ExtendedLangs.Swedish, "sv_SE.xml" }, // Custom
        { ExtendedLangs.Lithuanian, "lt_LT.xml" }, // Custom
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
        { ExtendedLangs.SChinese, "zh-CN" },
        { ExtendedLangs.TChinese, "zh-TW" },
        { ExtendedLangs.Irish, "ga-IE" },
        { ExtendedLangs.Polish, "pl-PL" }, // Custom
        { ExtendedLangs.Turkish, "tr-TR" }, // Custom
        { ExtendedLangs.Swedish, "sv-SE" }, // Custom
        { ExtendedLangs.Lithuanian, "lt-LT" }, // Custom
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
        { ExtendedLangs.SChinese, "zh" },
        { ExtendedLangs.TChinese, "zh" },
        { ExtendedLangs.Irish, "ga" },
        { ExtendedLangs.Polish, "pl" }, // Custom
        { ExtendedLangs.Turkish, "tr" }, // Custom
        { ExtendedLangs.Swedish, "sv" }, // Custom
        { ExtendedLangs.Lithuanian, "lt" }, // Custom
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

        text = Regex.Replace(text, @"\%([^%]+)\%", @"<$1>");
        if (text.Contains("\\<"))
        {
            text = text.Replace("\\<", "<");
        }

        if (text.Contains("\\>"))
        {
            text = text.Replace("\\>", ">");
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
            if (!LangList.ContainsValue(localeName + ".xml"))
            {
                Logger.LogError($"Invalid locale iso name: {localeName}");
                continue;
            }

            Logger.LogWarning($"Adding locale for: {localeName} in {file}");

            var language = LangList.FirstOrDefault(x => x.Value == localeName + ".xml").Key;
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