/// <author>Thomas Krahl</author>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace vipr.experiment.editor
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

        private void Start()
        {
            Setup(defaultColor);
        }

        public void Setup(Color color)
        {
            UpdateSliderValues(color.r, color.g, color.b);
            UpdatePreview();
            SetHexColor();
        }

        private void UpdateSliderValues(float r, float g, float b)
        {
            sliderRed.value = r;
            sliderGreen.value = g;
            sliderBlue.value = b;
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
            string r = Mathf.RoundToInt(sliderRed.value * 255f).ToString("X2");
            string g = Mathf.RoundToInt(sliderGreen.value * 255f).ToString("X2");
            string b = Mathf.RoundToInt(sliderBlue.value * 255f ).ToString("X2");

            hexColorInput.text = r + g + b; 
        }

        public Color GetColor()
        {
            return new Color(sliderRed.value, sliderGreen.value, sliderBlue.value);
        }

        

        public void OnRedInputChanged(string value)
        {
            float v = float.Parse(value) / 255f;
            labelRed.text = value;
            sliderRed.value = v;
            UpdatePreview();
            SetHexColor();
        }

        public void OnGreenInputChanged(string value)
        {
            float v = float.Parse(value) / 255f;
            labelGreen.text = value;
            sliderGreen.value = v;
            UpdatePreview();
            SetHexColor();
        }

        public void OnBlueInputChanged(string value)
        {
            float v = float.Parse(value) / 255f;
            labelBlue.text = value;
            sliderBlue.value = v;
            UpdatePreview();
            SetHexColor();
        }

        public void OnColorRedChanged(float value)
        {
            string v = Mathf.RoundToInt(value * 255f).ToString();
            labelRed.text = v.ToString();
            inputRed.text = v.ToString();
            UpdatePreview();
            SetHexColor();
        }

        public void OnColorGreenChanged(float value)
        {
            string v = Mathf.RoundToInt(value * 255f).ToString();
            labelGreen.text = v.ToString();
            inputGreen.text = v.ToString();
            UpdatePreview();
            SetHexColor();
        }

        public void OnColorBlueChanged(float value)
        {
            string v = Mathf.RoundToInt(value * 255f).ToString();
            labelBlue.text = v.ToString();
            inputBlue.text = v.ToString();
            UpdatePreview();
            SetHexColor();
        }

        private void UpdatePreview()
        {
            previewImage.color = new Color(sliderRed.value, sliderGreen.value, sliderBlue.value);
        }
    }
}

