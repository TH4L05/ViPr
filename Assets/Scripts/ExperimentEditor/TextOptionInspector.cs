/// <author>Thomas Krahl</author>

using eccon_lab.vipr.experiment.editor.ui;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment.editor
{
    public class TextOptionInspector : MonoBehaviour
    {
        [SerializeField] private ColorPicker textColor;
        [SerializeField] private Slider sliderSize;
        [SerializeField] private TextMeshProUGUI sliderSizeLabel;
        [SerializeField] private TMP_Dropdown dropdownStyle;
        [SerializeField] private TMP_Dropdown dropdownAlignmentH;
        [SerializeField] private TMP_Dropdown dropdownAlignmentV;
        [SerializeField] private TextMeshProUGUI previewlabel;

        private Dictionary<int, string> styles;
        private Dictionary<int, string> alignmentH;
        private Dictionary<int, string> alignmentV;

        private void Awake()
        {
            if (textColor != null) textColor.OnValueChanged += UpdateTextPreviewColor;
        }

        private void OnDestroy()
        {
            if (textColor != null) textColor.OnValueChanged -= UpdateTextPreviewColor;
        }

        public TextOptions GetTextValues()
        {
            TextOptions options = new TextOptions();
            options.textColor = GetTextColor();
            options.textSize = GetTextSizeValue();
            options.textStyle = GetStyleValue();
            options.horizontalAlignment = GetAlignmentHValue();
            options.verticalAlignment = GetAlignmentVValue();
            return options;
        }

        public Color GetTextColor()
        {
            if (textColor == null) return new Color(1, 1, 1);
            return textColor.GetColor();
        }

        public float GetTextSizeValue()
        {
            if (sliderSize == null) return 1f;
            return sliderSize.value;
        }

        public TMPro.FontStyles GetStyleValue()
        {
            if (dropdownStyle == null) return 0;
            return (TMPro.FontStyles)Enum.Parse(typeof(TMPro.FontStyles), styles[dropdownStyle.value]);
        }

        public TMPro.VerticalAlignmentOptions GetAlignmentVValue()
        {
            if (dropdownAlignmentH == null) return 0;
            return (VerticalAlignmentOptions)Enum.Parse(typeof(TMPro.VerticalAlignmentOptions), alignmentV[dropdownAlignmentV.value]);
        }

        public TMPro.HorizontalAlignmentOptions GetAlignmentHValue()
        {
            if (dropdownAlignmentV == null) return 0;
            return (HorizontalAlignmentOptions)Enum.Parse(typeof(TMPro.HorizontalAlignmentOptions), alignmentH[dropdownAlignmentH.value]);
        }

        public void SetTextValues(TextOptions textValues)
        {
            if(textColor != null) textColor.Setup(textValues.textColor);
            SetSliderValue(textValues.textSize);
            SetStyleDropdown(textValues.textStyle);
            SetAlignmentHDropdown(textValues.horizontalAlignment);
            SetAlignmentVDropdown(textValues.verticalAlignment);
        }

        public void SetSliderValue(float value)
        {
            if (sliderSize == null) return;
            sliderSize.value = value;
            UpdateTextPreviewSize(value);
        }

        public void SetStyleDropdown(TMPro.FontStyles currentStyle)
        {
            int index = 0;
            dropdownStyle.options.Clear();
            Array styleValues = Enum.GetValues(typeof(TMPro.FontStyles));
            styles = new Dictionary<int, string>();
            for (int i = 0; i < styleValues.Length-1; i++)
            {
                var value = styleValues.GetValue(i);
                styles.Add(i, value.ToString());
            }

            foreach (var styleValue in styles)
            {
                dropdownStyle.options.Add(new TMP_Dropdown.OptionData(styleValue.Value.ToString()));
                if (styleValue.Value.ToString() == currentStyle.ToString()) dropdownStyle.value = index;
                index++;
            }
            UpdateTextPreviewStyle(dropdownStyle.value);
        }

        public void SetAlignmentHDropdown(TMPro.HorizontalAlignmentOptions horizontalAlignment)
        {
            int index = 0;
            dropdownAlignmentH.options.Clear();
            Array values = Enum.GetValues(typeof(TMPro.HorizontalAlignmentOptions));
            alignmentH = new Dictionary<int, string>();
            for (int i = 0; i < values.Length - 1; i++)
            {
                var value = values.GetValue(i);
                alignmentH.Add(i, value.ToString());
            }

            foreach (var alignmentValue in alignmentH)
            {
                dropdownAlignmentH.options.Add(new TMP_Dropdown.OptionData(alignmentValue.Value.ToString()));
                if (alignmentValue.Value.ToString() == horizontalAlignment.ToString()) dropdownAlignmentH.value = index;
                index++;
            }
            UpdateTextAlignmentH(dropdownAlignmentH.value);
        }

        public void SetAlignmentVDropdown(TMPro.VerticalAlignmentOptions verticalAlignment)
        {
            int index = 0;
            dropdownAlignmentV.options.Clear();
            Array values = Enum.GetValues(typeof(TMPro.VerticalAlignmentOptions));
            alignmentV = new Dictionary<int, string>();
            for (int i = 0; i < values.Length - 1; i++)
            {
                var value = values.GetValue(i);
                alignmentV.Add(i, value.ToString());
            }

            foreach (var alignmentValue in alignmentV)
            {
                dropdownAlignmentV.options.Add(new TMP_Dropdown.OptionData(alignmentValue.Value.ToString()));
                if (alignmentValue.Value.ToString() == verticalAlignment.ToString()) dropdownAlignmentV.value = index;
                index++;
            }
            UpdateTextAlignmentV(dropdownAlignmentV.value);
        }

        public void UpdateTextPreviewColor(Color color)
        {
            previewlabel.color = color;
        }

        public void UpdateTextPreviewSize(float value)
        {
            previewlabel.fontSize = value;
            sliderSizeLabel.text = value.ToString();
        }

        public void UpdateTextPreviewStyle(int value)
        {
            previewlabel.fontStyle = (TMPro.FontStyles)Enum.Parse(typeof(TMPro.FontStyles), styles[value]);
        }

        public void UpdateTextAlignmentH(int value)
        {
            previewlabel.horizontalAlignment = (HorizontalAlignmentOptions)Enum.Parse(typeof(TMPro.HorizontalAlignmentOptions), alignmentH[value]);
        }

        public void UpdateTextAlignmentV(int value)
        {
            previewlabel.verticalAlignment = (VerticalAlignmentOptions)Enum.Parse(typeof(TMPro.VerticalAlignmentOptions), alignmentV[value]);
        }
    }
}

