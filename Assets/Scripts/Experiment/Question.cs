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
        [SerializeField] private TextValues textValues;
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
            textValues = new TextValues(Color.white, 30.0f);
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

        public TextValues GetTextValues()
        {
            return textValues;
        }

        public Color GetTextColor()
        {
            return textValues.textColor;
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

        public void SetTextValues(TextValues values)
        {
            textValues = values;
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
            SetTextObjectValues(textField, textValues);
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
            for (int i = 0; i < radioOptionValues.Length; i++)
            {
                Toggle toggle = assigendUiElement.transform.GetChild(1).GetChild(i).GetComponent<Toggle>();
                if (toggle == null) continue;

                toggle.isOn = false;
                if (!radioOptionValues[i].isEnabled)
                {
                    toggle.gameObject.SetActive(false);

                }
                else
                {
                    toggle.gameObject.SetActive(true);
                }

                if (radioOptionValues[i].isDefault)
                {
                    toggle.isOn = true;
                }
                
                TextMeshProUGUI toggleLabel = toggle.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                SetTextObjectValues(toggleLabel, textValues);
                toggleLabel.text = radioOptionValues[i].optionName;
            }
        }

        private void SetTextObjectValues(TextMeshProUGUI textField, TextValues textValues)
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
            slider.SetSliderLabelTextValues(textValues);
        }

        #endregion

        public ExperimentSaveDataQuestion GetSaveData()
        {
            ExperimentSaveDataQuestion questionData = new ExperimentSaveDataQuestion(id, name, questionType, assignedPageId, questionText, textValues, radioOptionValues, sliderOptions);
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
                    answer = assigendUiElement.transform.GetChild(1).GetComponentInChildren<TMP_InputField>().text;
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
