using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;

namespace LocalizeUs;

public static class LocaleUsAssets
{
    private const string ShortPath = "LocalizeUs.Resources";

    public static readonly AssetBundle MainBundle = AssetBundleManager.Load("locale-assets");

    public static TMP_FontAsset OpenSansFontTmp { get; } = MainBundle.LoadAsset<TMP_FontAsset>("NotoSans-VariableFont_wdth,wght SDF")!;

}