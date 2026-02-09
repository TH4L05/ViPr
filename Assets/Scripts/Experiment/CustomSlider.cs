/// <author>Thomas Krahl</author>

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using eccon_lab.vipr.experiment.editor;

namespace eccon_lab.vipr.experiment
{
    public class CustomSlider : MonoBehaviour
    {
        [SerializeField] private SliderOptions sliderOptions;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI sliderLabel;

        private string labelFormat;

        public void Start()
        {
            if (slider == null) return;
            Initialize();
        }

        public void Initialize()
        {
            Setup(sliderOptions);
        }

        public void Setup(SliderOptions sliderOptions)
        {
            this.sliderOptions = sliderOptions;
            slider.maxValue = sliderOptions.maxValue;
            slider.minValue = sliderOptions.minValue;
            slider.value = sliderOptions.defaultValue;
            SetSliderLabelFormat(sliderOptions.decimalPlaces);
            UpdateSliderLabel(sliderOptions.defaultValue);
        }

        public void OnSliderValueChanged(float value)
        {
            UpdateSliderLabel(value);
        }

        public void UpdateSliderLabel(float value)
        {
            if (sliderLabel == null) return;

            string prefix = "";
            if(sliderOptions.labelPrefix != string.Empty) prefix = sliderOptions.labelPrefix + " ";
            string suffix = "";
            if (sliderOptions.labelSuffix != string.Empty) suffix = " " + sliderOptions.labelSuffix;

            string text = prefix + value.ToString(labelFormat) + suffix;
            sliderLabel.text = text;
        }

        private void SetSliderLabelFormat(int decimals)
        {
            labelFormat = "";
            switch (decimals)
            {
                case 0:
                    slider.wholeNumbers = true;
                    break;
                case 1:
                    slider.wholeNumbers = false;
                    labelFormat = "0.0";
                    break;
                case 2:
                    slider.wholeNumbers = false;
                    labelFormat = "0.00";
                    break;
                case 3:
                    slider.wholeNumbers = false;
                    labelFormat = "0.000";
                    break;
                default:
                    break;
            }
        }

        public float GetSliderValue()
        {
            return slider.value;
        }

        public void SetSliderLabelTextValues(TextValues textValues)
        {
            sliderLabel.color = textValues.textColor;
            sliderLabel.fontSize = textValues.textSize;
            sliderLabel.fontStyle = textValues.textStyle;
            sliderLabel.horizontalAlignment = textValues.horizontalAlignment;
            sliderLabel.verticalAlignment = textValues.verticalAlignment;
        }
    }
}

