using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using BepInEx.Logging;
using Reactor.Localization;
using UnityEngine;

namespace LocalizeUs.Localization;

public static class CustomLocale
{
    public static string LocaleDirectory => Path.Combine(Application.persistentDataPath, "TownOfUs", "Locales");

    public static Dictionary<SupportedLangs, string> LangList { get; } = new()
    {
        { SupportedLangs.English, "en_US.xml" },
        { SupportedLangs.Latam, "es_419.xml" },
        { SupportedLangs.Brazilian, "pt_BR.xml" },
        { SupportedLangs.Portuguese, "pt_PT.xml" },
        { SupportedLangs.Korean, "ko_KR.xml" },
        { SupportedLangs.Russian, "ru_RU.xml" },
        { SupportedLangs.Dutch, "nl_NL.xml" },
        { SupportedLangs.Filipino, "fil_PH.xml" },
        { SupportedLangs.French, "fr_FR.xml" },
        { SupportedLangs.German, "de_DE.xml" },
        { SupportedLangs.Italian, "it_IT.xml" },
        { SupportedLangs.Japanese, "ja_JP.xml" },
        { SupportedLangs.Spanish, "es_ES.xml" },
        { SupportedLangs.SChinese, "zh_CN.xml" },
        { SupportedLangs.TChinese, "zh_TW.xml" },
        { SupportedLangs.Irish, "ga_IE.xml" },
        { (SupportedLangs)16, "pl_PL.xml" } // Polish
    };
    public static Dictionary<SupportedLangs, string> LangCultureList { get; } = new()
    {
        { SupportedLangs.English, "en-US" },
        { SupportedLangs.Latam, "es-419" },
        { SupportedLangs.Brazilian, "pt-BR" },
        { SupportedLangs.Portuguese, "pt-PT" },
        { SupportedLangs.Korean, "ko-KR" },
        { SupportedLangs.Russian, "ru-RU" },
        { SupportedLangs.Dutch, "nl-NL" },
        { SupportedLangs.Filipino, "fil-PH" },
        { SupportedLangs.French, "fr-FR" },
        { SupportedLangs.German, "de-DE" },
        { SupportedLangs.Italian, "it-IT" },
        { SupportedLangs.Japanese, "ja-JP" },
        { SupportedLangs.Spanish, "es-ES" },
        { SupportedLangs.SChinese, "zh-CN" },
        { SupportedLangs.TChinese, "zh-TW" },
        { SupportedLangs.Irish, "ga-IE" },
        { (SupportedLangs)16, "pl-PL" } // Polish
    };

    public static string BepinexLocaleDirectory =>
        Path.Combine(BepInEx.Paths.BepInExRootPath, "MiraLocales", "TownOfUs");

    /*public static Dictionary<string, StringNames> CustomLocaleList { get; } = [];*/

    public static Dictionary<string, string> TmpTextList { get; } = new()
    {
        { "<nl>", "\n" },
        { "<and>", "&" },
    };

    // Language, Xml Name, then Value
    public static Dictionary<SupportedLangs, Dictionary<string, string>> CustomLocalization { get; } = [];

    internal static ManualLogSource Logger { get; } = BepInEx.Logging.Logger.CreateLogSource("CustomLocale");

    public static string Get(string name, string? defaultValue = null)
    {
        var currentLanguage =
            TranslationController.InstanceExists
                ? TranslationController.Instance.currentLanguage.languageID
                : SupportedLangs.English;
        return Get(currentLanguage, name, defaultValue);
    }

    public static string Get(SupportedLangs language, string name, string? defaultValue = null)
    {
        if (CustomLocalization.TryGetValue(language, out var translations) &&
            translations.TryGetValue(name, out var translation))
        {
            return translation;
        }

        if (CustomLocalization.TryGetValue(SupportedLangs.English, out var translationsEng) &&
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
        return GetParsed(currentLanguage, name, defaultValue, parseList);
    }

    public static string GetParsed(SupportedLangs language, string name, string? defaultValue = null,
        Dictionary<string, string>? parseList = null)
    {
        var text = defaultValue ?? "STRMISS_" + name;

        if (CustomLocalization.TryGetValue(SupportedLangs.English, out var translationsEng) &&
            translationsEng.TryGetValue(name, out var translationEng))
        {
            text = translationEng;
        }

        if (language is not SupportedLangs.English && CustomLocalization.TryGetValue(language, out var translations) &&
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
                assembly.GetManifestResourceStream("TownOfUs.Resources.Locale." + locale.Value);
            if (resourceStream == null)
            {
                Logger.LogError($"Language is not added: {locale.Key.ToDisplayString()}");
                continue;
            }

            Logger.LogWarning($"Language is being added: {locale.Key.ToDisplayString()}");
            using StreamReader reader = new(resourceStream);
            string xmlContent = reader.ReadToEnd();

            CustomLocalization.TryAdd(locale.Key, []);
            ParseXmlFile(xmlContent, locale.Key);
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
            ParseXmlFile(xmlContent, language);
        }

        var translations = Directory.GetFiles(directory, "*.txt");
        foreach (var file in translations)
        {
            var localeName = Path.GetFileNameWithoutExtension(file);
            if (!Enum.TryParse<SupportedLangs>(localeName, out var language))
            {
                Logger.LogError($"Invalid locale name: {localeName}");
                continue;
            }

            CustomLocalization.TryAdd(language, []);
            ParseFile(file, language);
        }
    }

    public static void ParseFile(string file, SupportedLangs language)
    {
        foreach (var translation in File.ReadAllLines(file))
        {
            var parts = translation.Split('=');
            if (parts.Length >= 2)
            {
                var key = parts[0];
                var value = string.Join("=", parts.Skip(1));

                if (CustomLocalization[language].ContainsKey(key))
                {
                    var ogValuePair = CustomLocalization[language].FirstOrDefault(x => x.Key == key);
                    CustomLocalization[language].Remove(ogValuePair.Key);
                }

                CustomLocalization[language].TryAdd(key, value);
            }
            else
            {
                Logger.LogWarning("Invalid translation format: " + translation);
            }
        }
    }

    public static void ParseXmlFile(string xmlContent, SupportedLangs language)
    {
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

                        if (CustomLocalization[language].ContainsKey(name))
                        {
                            var ogValuePair = CustomLocalization[language].FirstOrDefault(x => x.Key == name);
                            CustomLocalization[language].Remove(ogValuePair.Key);
                        }

                        CustomLocalization[language].TryAdd(name, value);

                        /*if (language is SupportedLangs.English && !CustomLocaleList.ContainsKey(name))
                        {
                            var stringName = CustomStringName.CreateAndRegister(name);
                            CustomLocaleList.TryAdd(name, stringName);
                        }*/
                    }
                }

                Logger.LogWarning(
                    $"{CustomLocalization[language].Count} Localization strings added to {language.ToDisplayString()}!");
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