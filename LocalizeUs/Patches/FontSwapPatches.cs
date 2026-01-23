
/*
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalizeUs.Patches;

[HarmonyPatch]
public class FontSwapPatches
{
	public static Font ogBrookFont;
	public static TMP_FontAsset ogBrookTmp;

	public static void SwapTMPFont(ref TextMeshProUGUI __instance)
	{
		var auLang = (ExtendedLangs)(TranslationController.InstanceExists
			? TranslationController.Instance.currentLanguage.languageID
			: SupportedLangs.English);
		if (__instance.font.name == "LiberationSans-Regular SDF")
		{
			__instance.font = LocaleUsAssets.LibSansRegTmp;
		}
		else if (__instance.font.name == "Brook SDF" && CustomLocale.LangsWithCustomFont.Contains(auLang))
		{
			ogBrookTmp = __instance.font;
			__instance.font = LocaleUsAssets.AmaticScBoldTmp;
		}
		else if (__instance.font.name == "AmaticSC-Bold" && !CustomLocale.LangsWithCustomFont.Contains(auLang))
		{
			__instance.font = ogBrookTmp;
		}
		/*var currentLanguageCode = (CustomLocale.LangCodesList.GetValueOrDefault(auLang, "en"));
		bool isUnderlaid = __instance.gameObject.name.Contains("NameText") ||
		                   __instance.gameObject.name.Contains("LayerText") ||
		                   __instance.transform.parent.gameObject.name.Contains("Cheats Info");
		Vector4 originalUnderlaycolor = __instance.fontMaterial.GetVector("_UnderlayColor");#1#
		/*if (__instance.transform.parent.gameObject.name.Equals("Filler") &&
		    __instance.gameObject.name.Equals("HP Text") && __instance.GetComponentInParent<HealthBar>() != null)
		{
			return;
		}#1#
		/*
		switch (currentLanguageCode)
		{
			case "pl":
			{

				//Swap with a Chinese font when it comes in.
				__instance.font = Core.CJKFontTMP;
				if (isUnderlaid)
				{
					Material underlaid = new Material(__instance.fontMaterial);
					underlaid.SetVector("_UnderlayColor", originalUnderlaycolor);
					__instance.fontMaterial = underlaid;
				}
				else
				{
					__instance.fontSharedMaterial.SetVector("_UnderlayColor", new Vector4(0, 0, 0, 0));
				}

				break;
			}
			//Arabic Persian Urdu
			case "ar":
			case "fa":
			case "ur":
			{

				switch (__instance.alignment)
				{
					case TextAlignmentOptions.TopLeft:
						__instance.alignment = TextAlignmentOptions.TopRight;
						break;
					case TextAlignmentOptions.Left:
						__instance.alignment = TextAlignmentOptions.Right;
						break;
					case TextAlignmentOptions.BottomLeft:
						__instance.alignment = TextAlignmentOptions.BottomRight;
						break;
					case TextAlignmentOptions.BaselineLeft:
						__instance.alignment = TextAlignmentOptions.BaselineRight;
						break;
				}

				Core.GlobalFontTMP.fallbackFontAssetTable.Add(Core.ArabicFontTMP);

				if (GetCurrentSceneName() == "CreditsMuseum2")
				{
					if (__instance.font.name == "GFS Garaldus")
					{
						__instance.font = Core.MuseumFontTMP;
					}
					else
					{
						__instance.font = Core.GlobalFontTMP;
					}
				}
				else
				{
					__instance.font = Core.GlobalFontTMP;
					if (isUnderlaid)
					{
						Material underlaid = new Material(__instance.fontMaterial);
						underlaid.SetVector("_UnderlayColor", originalUnderlaycolor);
						__instance.fontMaterial = underlaid;
					}
					else
					{
						__instance.fontSharedMaterial.SetVector("_UnderlayColor", new Vector4(0, 0, 0, 0));
					}
				}

				break;
			}

			//Hebrew Yiddish Ladino Mozarabic Judeo-Arabic
			case "he":
			case "yi":
			case "la":
			case "ro":
			case "jr":
			{
				__instance.font = Core.HebrewFontTMP;
				if (isUnderlaid)
				{
					Material underlaid = new Material(__instance.fontMaterial);
					underlaid.SetVector("_UnderlayColor", originalUnderlaycolor);
					__instance.fontMaterial = underlaid;
				}
				else
				{
					__instance.fontSharedMaterial.SetVector("_UnderlayColor", new Vector4(0, 0, 0, 0));
				}

				break;
			}
		}#1#

	}

	static List<IntPtr> txtObjectsFixed = new List<IntPtr>();

	[HarmonyPatch(typeof(Text), "OnEnable")]
	[HarmonyPostfix]
	public static void SwapFontPostfix(ref Text __instance/*, IntPtr ___m_CachedPtr#1#)
	{
		/*if (txtObjectsFixed.Count > 0)
		{
			if (txtObjectsFixed.Contains(___m_CachedPtr))
			{
				return;
			}
		}#1#

		/*if (LanguageManager.IsRightToLeft)
		{
			switch (__instance.alignment)
			{
				case UpperLeft:
					__instance.alignment = UpperRight;
					break;
				case MiddleLeft:
					__instance.alignment = MiddleRight;
					break;
				case LowerLeft:
					__instance.alignment = LowerRight;
					break;
				case UpperRight:
					__instance.alignment = UpperLeft;
					break;
				case MiddleRight:
					__instance.alignment = MiddleLeft;
					break;
				case LowerRight:
					__instance.alignment = LowerLeft;
					break;
			}

			__instance.alignByGeometry = true;
		}#1#

		var auLang = (ExtendedLangs)(TranslationController.InstanceExists
			? TranslationController.Instance.currentLanguage.languageID
			: SupportedLangs.English);
		if (__instance.font.name == "LiberationSans-Regular")
		{
			__instance.font = LocaleUsAssets.LibSansRegFont;
		}
		else if (__instance.font.name == "Brook" && CustomLocale.LangsWithCustomFont.Contains(auLang))
		{
			ogBrookFont = __instance.font;
			__instance.font = LocaleUsAssets.AmaticScBoldFont;
		}
		else if (__instance.font.name == "AmaticSC-Bold" && !CustomLocale.LangsWithCustomFont.Contains(auLang))
		{
			__instance.font = ogBrookFont;
		}

		/*
		if (Core.GlobalFontReady)
		{
			if (GetCurrentSceneName() == "CreditsMuseum2")
			{
				if (__instance.font.fontNames[0] == "GFS Garaldus")
				{
					__instance.font = Core.MuseumFont;
				}
				else
				{
					__instance.font = Core.GlobalFont;
				}
			}
			else
			{
				originalFont = __instance.font;
				__instance.font = Core.GlobalFont;
			}
		}
		#1#

		//txtObjectsFixed.Add(___m_CachedPtr);
	}

	static List<IntPtr> tmpObjectsFixed = new List<IntPtr>();

	[HarmonyPatch(typeof(TextMeshProUGUI), "OnEnable")]
	[HarmonyPostfix]
	public static void TmpSwapFontPostfix(ref TextMeshProUGUI __instance/*, IntPtr ___m_CachedPtr#1#)
	{
		/*if (tmpObjectsFixed.Count > 0)
		{
			if (tmpObjectsFixed.Contains(___m_CachedPtr))
			{
				return;
			}
		}#1#


		/*if (Core.TMPFontReady)
		{
			if (isUsingEnglish())
			{
				if (GetCurrentSceneName() != "Main Menu")
				{
					return;
				}
			}

		}#1#
		SwapTMPFont(ref __instance);
		/*tmpObjectsFixed.Add(___m_CachedPtr);#1#

	}
}
*/
