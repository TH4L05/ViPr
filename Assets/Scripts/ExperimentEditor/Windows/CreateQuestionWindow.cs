/// <author>Thomas Krahl</author>

using System;
using TMPro;
using UnityEngine;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class CreateQuestionWindow : ExperimentEditorMenuWindow
    {
        #region Fields

        [SerializeField] private GameObject inputOptionQuestionText;
        [SerializeField] private GameObject inputOptionRadio;
        [SerializeField] private GameObject inputOptionSlider;
        [SerializeField] private TMP_Dropdown createQuestionDropdownType;
        [SerializeField] private TMP_InputField textQuestionText;
        [SerializeField] private RadioButtonCreateOption[] radioButtonOptions;
        [SerializeField] private SliderCreateOption sliderCreateOptions;

        #endregion

        public override void Initialize()
        {
            SetupQuestionTypeDropdown();
        }

        private void SetupQuestionTypeDropdown()
        {
            if (createQuestionDropdownType == null) return;
            foreach (var item in Enum.GetValues(typeof(QuestionType)))
            {
                createQuestionDropdownType.options.Add(new TMP_Dropdown.OptionData(item.ToString()));
            }
        }

        public override void ShowWindowContent()
        {
            textQuestionText.text = string.Empty;
            ResetRadioButtonOptions();
            ResetSliderOptions();
            OnQuestionTypeChanged(0);
        }

        public void OnQuestionTypeChanged(int value)
        {
            switch ((QuestionType)value)
            {
                case QuestionType.RadioButton:
                    if (inputOptionQuestionText != null) inputOptionQuestionText.SetActive(true);
                    if (inputOptionRadio != null) inputOptionRadio.SetActive(true);
                    if (inputOptionSlider != null) inputOptionSlider.SetActive(false);
                    break;
                case QuestionType.InputField:
                    if (inputOptionQuestionText != null) inputOptionQuestionText.SetActive(true);
                    if (inputOptionRadio != null) inputOptionRadio.SetActive(false);
                    break;
                case QuestionType.Slider:
                    if (inputOptionQuestionText != null) inputOptionQuestionText.SetActive(true);
                    if (inputOptionRadio != null) inputOptionRadio.SetActive(false);
                    if (inputOptionSlider != null) inputOptionSlider.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void ResetRadioButtonOptions()
        {
            foreach (var item in radioButtonOptions)
            {
                item.Reset();
            }
        }

        public void ResetSliderOptions()
        {
            sliderCreateOptions.Reset();
        }

        public override void OnButtonClick()
        {
            base.OnButtonClick();
            Debug.Log("Create new question");

            if (radioButtonOptions[0].optionToggle.isOn && radioButtonOptions[0].defaultOption != null)
            {
                radioButtonOptions[0].defaultOption.isOn = true;
            }

            ExperimentEditor.Instance.CreateNewQuestion((QuestionType)createQuestionDropdownType.value, textQuestionText.text, radioButtonOptions, sliderCreateOptions);
        }
    }
}

