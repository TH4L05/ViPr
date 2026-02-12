
using TMPro;
using UnityEngine;

namespace eccon_lab.vipr.experiment
{
    public class InputAnswerHandle : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI infoLabel;
        private int characterCount;
        private int characterCountMax;

        private void OnEnable()
        {
            characterCountMax = inputField.characterLimit;
            inputField.text = string.Empty;
            UpdateCharacterInfo();
        }

        public string GetInputText()
        {
            return inputField.text;
        }

        public void OnInputValueChanged()
        {
            UpdateCharacterCount();
        }

        private void UpdateCharacterCount()
        {
            string text = inputField.text;
            characterCount = text.ToCharArray().Length;
            UpdateCharacterInfo();
        }

        private void UpdateCharacterInfo()
        {
            infoLabel.text = characterCount.ToString() + "/" + characterCountMax.ToString();
        }
    }
}

