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
        [SerializeField] private RadioButtonCreateOptions radioButtonCreateOptions;
        [SerializeField] private SliderCreateOption sliderCreateOptions;

        #endregion

        public override void Initialize()
        {
            SetupQuestionTypeDropdown();
            TextOptions textOptions = ExperimentEditor.Instance.CurrentExperiment.DefaultTextValues;
            radioButtonCreateOptions.textOptionInspector.SetTextValues(textOptions);
            sliderCreateOptions.textOptionInspector.SetTextValues(textOptions);
            radioButtonCreateOptions.ResetOptionValues();
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
            radioButtonCreateOptions.ResetOptionValues();
            
        }

        public void ResetSliderOptions()
        {
            sliderCreateOptions.Reset();
        }

        public override void OnButtonClick()
        {
            base.OnButtonClick();
            Debug.Log("Create new question");
            RadioButtonOptions radioButtonOptions = radioButtonCreateOptions.GetValues();
            radioButtonOptions.radioOptionValues[0].isDefault = true;
            ExperimentEditor.Instance.CreateNewQuestion((QuestionType)createQuestionDropdownType.value, textQuestionText.text, radioButtonOptions, sliderCreateOptions.GetValues());
        }
    }
}

