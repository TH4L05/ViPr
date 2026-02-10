/// <author>Thomas Krahl</author>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class ColorPicker : MonoBehaviour
    {
        [SerializeField] private Slider sliderRed;
        [SerializeField] private Slider sliderGreen;
        [SerializeField] private Slider sliderBlue;
        [SerializeField] private TMP_InputField inputRed;
        [SerializeField] private TMP_InputField inputGreen;
        [SerializeField] private TMP_InputField inputBlue;
        [SerializeField] private TextMeshProUGUI labelRed;
        [SerializeField] private TextMeshProUGUI labelGreen;
        [SerializeField] private TextMeshProUGUI labelBlue;
        [SerializeField] private Image previewImage;
        [SerializeField] private TMP_InputField hexColorInput;
        [SerializeField] private Color defaultColor = new Color(0.1538461f, 0.06153846f, 0.2f, 1f);

        public void Initialize()
        {
            Debug.Log("Initialize Color Picker");
            UpdateSliderValues(defaultColor.r, defaultColor.g, defaultColor.b);
            UpdatePreview();
            SetHexColor();
        }

        public void Setup(Color color)
        {
            Debug.Log("Set Color Picker color= " + color.r + "/" + color.g + "/" + color.b);
            UpdateSliderValues(color.r, color.g, color.b);
            UpdatePreview();
            SetHexColor();
        }

        private void UpdateSliderValues(float r, float g, float b)
        {
            if (sliderRed != null) sliderRed.value = r;
            if (sliderGreen != null) sliderGreen.value = g;
            if (sliderBlue != null) sliderBlue.value = b;
            OnColorRedChanged(r);
            OnColorGreenChanged(g);
            OnColorBlueChanged(b);
        }

        public void OnHexColorInputChanged()
        {
            string hex = hexColorInput.text;

            string r = hex.Substring(0, 2);
            string g = hex.Substring(2, 2);
            string b = hex.Substring(4, 2);

            float rValue = int.Parse(r, System.Globalization.NumberStyles.HexNumber);
            float gValue = int.Parse(g, System.Globalization.NumberStyles.HexNumber);
            float bValue = int.Parse(b, System.Globalization.NumberStyles.HexNumber);

            UpdateSliderValues(rValue / 255f, gValue / 255f, bValue / 255f);
            UpdatePreview();
        }

        private void SetHexColor()
        {
            if(hexColorInput == null) return;
            string r = Mathf.RoundToInt(sliderRed.value * 255f).ToString("X2");
            string g = Mathf.RoundToInt(sliderGreen.value * 255f).ToString("X2");
            string b = Mathf.RoundToInt(sliderBlue.value * 255f ).ToString("X2");
            hexColorInput.text = r + g + b; 
        }

        public Color GetColor()
        {
            return new Color(sliderRed.value, sliderGreen.value, sliderBlue.value);
        }

        public void OnRedInputChanged(string inputValue)
        {
            float value = float.Parse(inputValue);

            if(value < 0f) value = 0f;
            if(value > 255f) value = 255f;
            if(labelRed != null) labelRed.text = inputValue;
            if(sliderRed != null) sliderRed.value = value / 255f;
            UpdatePreview();
            SetHexColor();
        }

        public void OnGreenInputChanged(string inputValue)
        {
            float value = float.Parse(inputValue);
            if (value < 0f) value = 0f;
            if (value > 255f) value = 255f;
            if (labelGreen != null) labelGreen.text = inputValue;
            if (sliderGreen != null) sliderGreen.value = value / 255f;
            UpdatePreview();
            SetHexColor();
        }

        public void OnBlueInputChanged(string inputValue)
        {
            float value = float.Parse(inputValue);
            if (value < 0f) value = 0f;
            if (value > 255f) value = 255f;
            if (labelBlue != null) labelBlue.text = inputValue;
            if (sliderBlue != null) sliderBlue.value = value / 255f;
            UpdatePreview();
            SetHexColor();
        }

        public void OnColorRedChanged(float value)
        {
            string textValue = Mathf.RoundToInt(value * 255f).ToString();
            if (labelRed != null) labelRed.text = textValue.ToString();
            if (inputRed != null) inputRed.text = textValue.ToString();
            UpdatePreview();
            SetHexColor();
        }

        public void OnColorGreenChanged(float value)
        {
            string textValue = Mathf.RoundToInt(value * 255f).ToString();
            if (labelGreen != null) labelGreen.text = textValue.ToString();
            if (inputGreen != null) inputGreen.text = textValue.ToString();
            UpdatePreview();
            SetHexColor();
        }

        public void OnColorBlueChanged(float value)
        {
            string textValue = Mathf.RoundToInt(value * 255f).ToString();
            if (labelBlue != null) labelBlue.text = textValue.ToString();
            if (inputBlue != null) inputBlue.text = textValue.ToString();
            UpdatePreview();
            SetHexColor();
        }

        private void UpdatePreview()
        {
            if (previewImage == null) return;
            previewImage.color = new Color(sliderRed.value, sliderGreen.value, sliderBlue.value);
        }
    }
}

