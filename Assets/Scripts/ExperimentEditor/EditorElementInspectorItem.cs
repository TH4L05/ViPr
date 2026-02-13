/// <author>Thomas Krahl</author>

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using eccon_lab.vipr.experiment.editor.ui;

namespace eccon_lab.vipr.experiment.editor
{
    public class EditorElementInspectorItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TMP_InputField inputField; 
        [SerializeField] private ColorPicker colorPicker;
        [SerializeField] private TextOptionInspector textOptionInspector;
        [SerializeField] private RadioButtonCreateOptions radioCreateOptionValues;
        [SerializeField] private ToggleGroup radioOptionSetDefaultGroup;
        [SerializeField] private SliderCreateOption sliderCreateOptions;
        
        public void Initialize()
        {
            if(colorPicker != null) colorPicker.Initialize();
        }

        public void SetLabelText(string text)
        {
            label.text = text;
        }

        public string GetInputValue()
        {
            if(inputField  == null) return string.Empty;
            return inputField.text;
        }

        public void SetInput(string value)
        {
            if (inputField == null) return;
            inputField.text = value;
        }

        public void SetColorValue(Color color)
        {
            if (colorPicker == null) return;
            colorPicker.Setup(color);
        }

        public Color GetColorValue()
        {
            if (colorPicker == null) return Color.purple;
            return colorPicker.GetColor();
        }

        public void SetTextOptions(TextOptions textValues)
        {
            textOptionInspector.SetTextValues(textValues); 
        }

        public TextOptions GetTextOptions()
        {
            return textOptionInspector.GetTextValues();
        }

        public void SetSliderOptions(SliderOptions sliderOptions)
        {
            sliderCreateOptions.sliderMaxValue.text = sliderOptions.maxValue.ToString();
            sliderCreateOptions.sliderMinValue.text = sliderOptions.minValue.ToString();
            sliderCreateOptions.sliderDefaultValue.text = sliderOptions.defaultValue.ToString();
            sliderCreateOptions.sliderLabelPrefix.text = sliderOptions.labelPrefix.ToString();
            sliderCreateOptions.sliderLabelSuffix.text = sliderOptions.labelSuffix.ToString();
            sliderCreateOptions.decimalPlaces.value  = sliderOptions.decimalPlaces;
            sliderCreateOptions.SetTextOptions(sliderOptions.textOptions);
        }

        public SliderOptions GetSliderOptions()
        {
            SliderOptions options = new SliderOptions( 
                                                        sliderCreateOptions.textOptionInspector.GetTextValues(),
                                                        float.Parse(sliderCreateOptions.sliderMinValue.text), 
                                                        float.Parse(sliderCreateOptions.sliderMaxValue.text), 
                                                        float.Parse(sliderCreateOptions.sliderDefaultValue.text),
                                                        sliderCreateOptions.sliderLabelPrefix.text,
                                                        sliderCreateOptions.sliderLabelSuffix.text,
                                                        (int)sliderCreateOptions.decimalPlaces.value
                                                      );
            return options;
        }

        public void SetRadioButtonOptions(RadioButtonOptions options)
        {
            for (int i = 0; i < options.radioOptionValues.Length; i++)
            {
                radioCreateOptionValues.radioButtonCreateOptionElements[i].SetValues(options.radioOptionValues[i]);
            }
            radioCreateOptionValues.textOptionInspector.SetTextValues(options.textOptions);
        }

        public RadioButtonOptions GetRadioOptionValues()
        {
            return radioCreateOptionValues.GetValues();
        }
    }
}

