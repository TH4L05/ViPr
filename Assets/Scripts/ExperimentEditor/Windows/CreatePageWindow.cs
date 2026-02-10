using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class CreatePageWindow : ExperimentEditorMenuWindow
    {
        [SerializeField] private TMP_Dropdown dropdownType;
        [SerializeField] private TMP_InputField inputOptionQuestionText;
        [SerializeField] private ColorPicker backgroundColor;
        [SerializeField] private TextOptionInspector textOptions;
        [SerializeField] private GameObject textOptionsObject;

        public override void Initialize()
        {
            
            backgroundColor.Initialize();
            SetupTypeDropdown();
            OnDropdownValueChanged(0);
        }

        public override void ShowWindowContent()
        {
            textOptions.SetTextValues(ExperimentEditor.Instance.CurrentExperiment.DefaultTextValues);
            inputOptionQuestionText.text = string.Empty;
            OnDropdownValueChanged(0);
        }

        public void OnDropdownValueChanged(int value)
        {
            switch ((PageType)value)
            {
                case PageType.ContentPage:
                    inputOptionQuestionText.gameObject.SetActive(false);
                    textOptionsObject.SetActive(false);
                    break;
                case PageType.InfoPage:
                    inputOptionQuestionText.gameObject.SetActive(true);
                    textOptionsObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        private void SetupTypeDropdown()
        {
            if (dropdownType == null) return;
            foreach (var item in Enum.GetValues(typeof(PageType)))
            {
                dropdownType.options.Add(new TMP_Dropdown.OptionData(item.ToString()));
            }
        }

        public void OnCreateButtonClicked()
        {
            ExperimentEditor.Instance.CreateNewPage((PageType)dropdownType.value, backgroundColor.GetColor(), textOptions.GetTextValues(), "", inputOptionQuestionText.text);
        }
    }
}

