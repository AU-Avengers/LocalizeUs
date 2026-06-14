using System.Globalization;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using Reactor;
using Reactor.Utilities;

namespace LocalizeUs;

[BepInAutoPlugin("auavengers.localize.us", "Localize Us!")]
[BepInDependency(ReactorPlugin.Id)]
public sealed partial class LocalizeUsPlugin : BasePlugin
{
    /// <summary>
    ///     Gets the specified Culture for string manipulations.
    /// </summary>
    public static CultureInfo Culture { get; internal set; } = new("en-US");

    public LocalizeUsPlugin()
    {
        CustomLocale.Initialize();
    }

    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        EnumInjector.InjectEnumValues<StringNames>(new Dictionary<string, object>
        {
            { "LangPolish", 2860 }, { "LangTurkish", 2861 }, { "LangSwedish", 2862 }, { "LangLithuanian", 2863 },
            { "LangCzech", 2864 },
            { "LangLiteraryChinese", 2865 },
            { "LangGreek", 2866 }
        });
        EnumInjector.InjectEnumValues<SupportedLangs>(new Dictionary<string, object>
        {
            { "Polish", (int)ExtendedLangs.Polish }, { "Turkish", (int)ExtendedLangs.Turkish },
            { "Swedish", (int)ExtendedLangs.Swedish }, { "Lithuanian", (int)ExtendedLangs.Lithuanian },
            { "Czech", (int)ExtendedLangs.Czech }, { "LiteraryChinese", (int)ExtendedLangs.LiteraryChinese },
            { "Greek", (int)ExtendedLangs.Greek }
        });
        ReactorCredits.Register<LocalizeUsPlugin>(location =>
            location == ReactorCredits.Location.MainMenu || location == ReactorCredits.Location.PingTracker);

        Harmony.PatchAll();
    }
}
