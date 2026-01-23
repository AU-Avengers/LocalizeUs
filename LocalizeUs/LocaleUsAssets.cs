using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;

namespace LocalizeUs;

public static class LocaleUsAssets
{
    public static readonly AssetBundle MainBundle = AssetBundleManager.Load("locale-assets");

    public static TMP_FontAsset AmaticScBoldTmp { get; } = MainBundle.LoadAsset<TMP_FontAsset>("AmaticSC-Bold SDF")!;
    public static TMP_FontAsset LibSansRegTmp { get; } =
        MainBundle.LoadAsset<TMP_FontAsset>("LiberationSans Extended SDF")!;

}
