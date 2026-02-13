using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment.editor
{
    public class RadioButtonCreateOption : MonoBehaviour
    {
        public TMP_InputField optionInputText;
        public Toggle optionToggle;
        public Toggle defaultOption;

        public void ResetOption()
        {
            optionInputText.text = string.Empty;
            optionToggle.isOn = false;
            if (defaultOption != null) defaultOption.isOn = false;
        }

        public void SetValues(string optionName, bool isEnabled, bool isDefault)
        {
            optionInputText.text = optionName;
            optionToggle.isOn = isEnabled;
            if(defaultOption != null) defaultOption.isOn = isDefault;
        }

        public void SetValues(RadioOptionValue options)
        {
            optionInputText.text = options.optionName;
            optionToggle.isOn = options.isEnabled;
            if (defaultOption != null) defaultOption.isOn = options.isEnabled;
        }

        public RadioOptionValue GetValues()
        {
            RadioOptionValue value = new RadioOptionValue();
            value.optionName = optionInputText.text;
            value.isEnabled = optionToggle.isOn;
            value.isDefault = false;
            if (defaultOption != null)
            {
                value.isDefault = defaultOption.isOn;
            }
            return value;
        }
    }
}

