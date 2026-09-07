using LocalizeUs;
using Reactor.Utilities.Attributes;
using TMPro;
using UnityEngine;

namespace RTLTMPro
{
    [RegisterInIl2Cpp]
    public class RTLTextMeshPro3D(IntPtr cppPtr) : MonoBehaviour(cppPtr)
    {
        public TextMeshPro TmpText;
        public TextTranslatorTMP TmpTranslator;
        // ReSharper disable once InconsistentNaming
#if TMP_VERSION_2_1_0_OR_NEWER
        public override string text
#else
        public new string text
#endif
        {
            get { return TmpText.text; }
            set
            {
                if (originalText == value)
                    return;

                originalText = value;

                UpdateText();
            }
        }

        public string OriginalText
        {
            get { return originalText; }
        }

        public bool PreserveNumbers
        {
            get { return preserveNumbers; }
            set
            {
                if (preserveNumbers == value)
                    return;

                preserveNumbers = value;
                TmpText.havePropertiesChanged = true;
            }
        }

        public bool Farsi
        {
            get { return farsi; }
            set
            {
                if (farsi == value)
                    return;

                farsi = value;
                TmpText.havePropertiesChanged = true;
            }
        }

        public bool FixTags
        {
            get { return fixTags; }
            set
            {
                if (fixTags == value)
                    return;

                fixTags = value;
                TmpText.havePropertiesChanged = true;
            }
        }

        protected bool preserveNumbers;

        protected bool farsi = true;

        protected string originalText;

        protected bool fixTags = true;

        protected readonly FastStringBuilder finalText = new (RTLSupport.DefaultBufferSize);

        public bool HavePropsChanged;
        protected void Update()
        {
            HavePropsChanged = TmpText.havePropertiesChanged;
        }
        protected void LateUpdate()
        {
            if (CustomLocale.IsRightToLeftLanguage())
            {
                return;
            }
            if (TmpText.havePropertiesChanged)
            {
                UpdateText();
            }
        }

        public void Awake()
        {
            TmpText = GetComponent<TextMeshPro>();
            originalText = TmpText.text;
            if (TryGetComponent(out TmpTranslator))
            {
                originalText = TranslationController.InstanceExists
                    ? TranslationController.Instance.GetString(TmpTranslator.TargetText)
                    : TmpText.text;
            }
        }

        public void UpdateText()
        {
            originalText = TmpText.text;
            if (TmpTranslator)
            {
                originalText = TranslationController.InstanceExists
                    ? TranslationController.Instance.GetString(TmpTranslator.TargetText)
                    : TmpText.text;
            }

            if (!TextUtils.IsRTLInput(originalText))
            {
                TmpText.isRightToLeftText = false;
                TmpText.text = originalText;
            }
            else
            {
                TmpText.isRightToLeftText = true;
                TmpText.text = GetFixedText(originalText);
            }

            TmpText.havePropertiesChanged = false;
        }

        private string GetFixedText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            finalText.Clear();
            RTLSupport.FixRTL(input, finalText, farsi, fixTags, preserveNumbers);
            finalText.Reverse();

            return finalText.ToString();
        }
    }
}