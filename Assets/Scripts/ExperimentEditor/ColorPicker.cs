/// <author>Thomas Krahl</author>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace vipr.experiment.editor
{
    public class ColorPicker : MonoBehaviour
    {
        [SerializeField] private Slider silderRed;
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

        private Color defaultColor = new Color(0.1538461f, 0.06153846f, 0.2f, 1f);

        public void Setup(Color color)
        {
            UpdateSliderValues(color.r, color.g, color.b);
            UpdatePreview();
            SetHexColor();
        }

        private void UpdateSliderValues(float r, float g, float b)
        {
            silderRed.value = r;
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
            string r = Mathf.RoundToInt(silderRed.value * 255f).ToString("X");
            string g = Mathf.RoundToInt(sliderGreen.value * 255f).ToString("X");
            string b = Mathf.RoundToInt(sliderBlue.value * 255f ).ToString("X");

            hexColorInput.text = r + g + b; 
        }

        public Color GetColor()
        {
            return new Color(silderRed.value, sliderGreen.value, sliderBlue.value);
        }

        public void OnColorRedChanged(float value)
        {
            string v = Mathf.RoundToInt(value * 255f).ToString();
            labelRed.text = v.ToString();
            UpdatePreview();
            SetHexColor();
        }

        public void OnRedInputChanged(string value)
        {
            OnColorRedChanged(float.Parse(value) / 255f);
        }

        public void OnColorGreenChanged(float value)
        {
            string v = Mathf.RoundToInt(value * 255f).ToString();
            labelGreen.text = v.ToString();
            UpdatePreview();
            SetHexColor();
        }

        public void OnColorBlueChanged(float value)
        {
            string v = Mathf.RoundToInt(value * 255f).ToString();
            labelBlue.text = v.ToString();
            UpdatePreview();
            SetHexColor();
        }

        private void UpdatePreview()
        {
            previewImage.color = new Color(silderRed.value, sliderGreen.value, sliderBlue.value);
        }
    }
}

