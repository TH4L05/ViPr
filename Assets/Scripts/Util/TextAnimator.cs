/// <author>Thomas Krahl</author>

using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace eecon_lab.Utilities
{
    public class TextAnimator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField, Range(0.001f, 2f)] private float speed = 1.0f;
        private char[] characters;
        private string displayText;
        private char[] charactersbackwards;

        private void Start()
        {
            characters = textField.text.ToCharArray();
            charactersbackwards = characters.Reverse().ToArray();
            textField.text = string.Empty;
        }

        public void AnimateByCharForward()
        {
            displayText = string.Empty;
            StartCoroutine(DisplayStringF(speed));
        }

        public void AnimateByCharBackward()
        {
            Debug.Log("Start reverse");
            StartCoroutine(DisplayStringB(speed));
        }

        IEnumerator DisplayStringF(float speed)
        {
            foreach (var character in characters)
            {
                displayText += character;
                textField.text = displayText;
                yield return new WaitForSeconds(speed);
            }
        }

        IEnumerator DisplayStringB(float speed)
        {
            Debug.Log("Start");
            for (int i = 0; i < characters.Length; i++)
            {
                int index = characters.Length - 1 - i;
                if(index <0 ) index = 0;
                displayText = displayText.Remove(index);
                textField.text = displayText;
                yield return new WaitForSeconds(speed);
            }
        }

        public void ClearText()
        {
            textField.text = string.Empty;
        }
    }
}

