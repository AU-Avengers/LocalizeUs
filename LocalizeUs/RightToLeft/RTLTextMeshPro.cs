using Reactor.Utilities.Attributes;
using TMPro;
using UnityEngine;

namespace RTLTMPro
{
    [RegisterInIl2Cpp]
    public class RTLTextMeshPro(IntPtr cppPtr) : MonoBehaviour(cppPtr)
    {
        public TextMeshProUGUI TmpText;
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

        public bool ForceFix
        {
            get { return forceFix; }
            set
            {
                if (forceFix == value)
                    return;

                forceFix = value;
                TmpText.havePropertiesChanged = true;
            }
        }

        protected bool preserveNumbers;

        protected bool farsi = true;

        protected string originalText;

        protected bool fixTags = true;

        protected bool forceFix;

        protected readonly FastStringBuilder finalText = new FastStringBuilder(RTLSupport.DefaultBufferSize);

        protected void Update()
        {
            if (TmpText.havePropertiesChanged)
            {
                UpdateText();
            }
        }

        public void Awake()
        {
            TmpText = GetComponent<TextMeshProUGUI>();
        }

        public void UpdateText()
        {
            if (originalText == null)
                originalText = "";

            if (ForceFix == false && TextUtils.IsRTLInput(originalText) == false)
            {
                TmpText.isRightToLeftText = false;
                TmpText.text = originalText;
            } else
            {
                TmpText.isRightToLeftText = true;
                TmpText.text = GetFixedText(originalText);
            }

            TmpText.havePropertiesChanged = true;
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