/// <author>Thomas Krahl</author>

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using eccon_lab.vipr.experiment.editor;

namespace eccon_lab.vipr.experiment
{
    [System.Serializable]
    public class Question : ExperimentElement
    {
        #region SerializedFields

        [SerializeField] private string assignedPageId;
        [SerializeField] private QuestionType questionType;
        [SerializeField] private string questionText;
        [SerializeField] private TextOptions textOptions;
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 rotation;
        [SerializeField] private Vector2 size;
        [SerializeField] private RadioOptionValue[] radioOptionValues;
        [SerializeField] private SliderOptions sliderOptions;

        #endregion

        #region PrivateFields
        #endregion

        #region PublicFields

        public string AssignedPageId => assignedPageId;

        #endregion

        #region Setup

        override public void Initialize(string id, string name, GameObject assignedUiElement)
        {
            base.Initialize(name, id, assignedUiElement);
            radioOptionValues = new RadioOptionValue[8];
            sliderOptions = new SliderOptions(1.0f, 10.0f, 5f);
            textOptions = new TextOptions(Color.white, 30.0f);
            position = assigendUiElement.GetComponent<RectTransform>().position;
            rotation = assigendUiElement.GetComponent<RectTransform>().rotation.eulerAngles;
            size = assigendUiElement.GetComponent<RectTransform>().sizeDelta;
        }

        public void QuestionSetup(QuestionType type, string referencePageId, string text)
        {
            Debug.Log("Question Setup");
            questionText = text;
            questionType = type;
            assignedPageId = referencePageId;
            SetupAssignedObject();
        }

        #endregion

        #region Get

        public QuestionType GetQuestionType()
        {
            return questionType;
        }

        public TextOptions GetTextValues()
        {
            return textOptions;
        }

        public Color GetTextColor()
        {
            return textOptions.textColor;
        }

        public string GetQuestionText()
        {
            return questionText;
        }

        public RadioOptionValue[] GetRadioOptionValues()
        {

            return radioOptionValues;
        }

        public SliderOptions GetSliderOptionValues()
        {
            return sliderOptions;
        }

        #endregion

        #region Set

        public void SetTextValues(TextOptions values)
        {
            textOptions = values;
        }

        public void SetQuestionText(string text)
        {
            questionText = text;
        }

        public void SetSliderOptions(SliderOptions options)
        {
            sliderOptions = options;
        }

        public void SetRadioOptionValues(RadioOptionValue[] optionValues)
        {
            radioOptionValues = optionValues;
        }

        #endregion

        #region UiElement

        public void SetupAssignedObject()
        {
            TextMeshProUGUI textField = assigendUiElement.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            SetTextObjectValues(textField, textOptions);
            textField.text = questionText;

            switch (questionType)
            {
                case QuestionType.RadioButton:
                    SetupAssignedObjectRadioButtons();
                    break;
                case QuestionType.InputField:
                    break;
                case QuestionType.Slider:
                    SetupAssignedObjectSlider();
                    break;
                default:
                    break;
            }
        }

        public void SetupAssignedObjectRadioButtons()
        {
            if (radioOptionValues.Length == 0 || radioOptionValues == null) return;

            Toggle[] toggleElements = assigendUiElement.transform.GetChild(1).GetComponent<ToggleGroupHandler>().GetToggleElements();

            for (int i = 0; i < radioOptionValues.Length; i++)
            {
                if (!radioOptionValues[i].isEnabled)
                {
                    toggleElements[i].gameObject.SetActive(false);

                }
                else
                {
                    toggleElements[i].gameObject.SetActive(true);
                }
                toggleElements[i].isOn = radioOptionValues[i].isDefault;
                TextMeshProUGUI toggleLabel = toggleElements[i].gameObject.GetComponentInChildren<TextMeshProUGUI>();
                SetTextObjectValues(toggleLabel, textOptions);
                toggleLabel.text = radioOptionValues[i].optionName;
            }
        }

        private void SetTextObjectValues(TextMeshProUGUI textField, TextOptions textValues)
        {
            textField.color = textValues.textColor;
            textField.fontSize = textValues.textSize;
            textField.fontStyle = textValues.textStyle;
            textField.horizontalAlignment = textValues.horizontalAlignment;
            textField.verticalAlignment = textValues.verticalAlignment;
        }

        public void SetupAssignedObjectSlider()
        {
            CustomSlider slider = assigendUiElement.transform.GetChild(1).GetChild(0).GetComponent<CustomSlider>();
            if (slider == null) return;
            slider.Setup(sliderOptions);
            slider.SetSliderLabelTextValues(textOptions);
        }

        #endregion

        public ExperimentSaveDataQuestion GetSaveData()
        {
            ExperimentSaveDataQuestion questionData = new ExperimentSaveDataQuestion(id, name, questionType, assignedPageId, questionText, textOptions, radioOptionValues, sliderOptions);
            return questionData;
        }

        public string GetAnswerValue()
        {
            string answer = "";

            switch (questionType)
            {
                case QuestionType.RadioButton:
                    answer = assigendUiElement.transform.GetChild(1).GetComponent<ToggleGroupHandler>().GetActiveValue();
                    break;
                case QuestionType.InputField:
                    answer = assigendUiElement.transform.GetChild(1).GetComponent<InputAnswerHandle>().GetInputText();
                    break;
                case QuestionType.Slider:
                    answer = assigendUiElement.transform.GetChild(1).GetComponentInChildren<CustomSlider>().GetSliderValue().ToString();
                    break;
                default:
                    break;
            }

            return answer;
        }
    }

    
}
