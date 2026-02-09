/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace eccon_lab.vipr
{
    public class ButtonImageToggle : MonoBehaviour
    {
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite spriteDefault;
        [SerializeField] private Sprite spriteToggle;
        [SerializeField] private TextMeshProUGUI buttonTextField;
        [SerializeField] private string buttonTextDefault;
        [SerializeField] private string buttonTextToggle;

        private void Start()
        {
            buttonImage = GetComponent<Image>();
            spriteDefault = buttonImage.sprite;
            buttonTextField = GetComponentInChildren<TextMeshProUGUI>();
            buttonTextDefault = buttonTextField.text;
        }

        public void ToggleSprite(bool defaultSprite)
        {
            if (defaultSprite)
            {
                buttonImage.sprite = spriteDefault;
                buttonTextField.text = buttonTextDefault;
                return;
            }
            buttonImage.sprite = spriteToggle;
            buttonTextField.text = buttonTextToggle;
        }
    }
}