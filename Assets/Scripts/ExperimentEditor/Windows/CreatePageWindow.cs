using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class CreatePageWindow : ExperimentEditorMenuWindow
    {
        [SerializeField] private TMP_Dropdown dropdownType;
        [SerializeField] private TMP_InputField inputOptionPageText;
        [SerializeField] private GameObject inputOptionPageTextObject;
        [SerializeField] private ColorPicker backgroundColor;
        [SerializeField] private TextOptionInspector textOptions;
        [SerializeField] private GameObject textOptionsObject;

        public override void Initialize()
        {
            base.Initialize();
            backgroundColor.Initialize();
            SetupTypeDropdown();
            OnDropdownValueChanged(0);
        }

        public override void ShowWindowContent()
        {
            textOptions.SetTextValues(ExperimentEditor.Instance.CurrentExperiment.DefaultTextValues);
            backgroundColor.Setup(ExperimentEditor.Instance.CurrentExperiment.DefaultPageBackgroundColor);
            inputOptionPageText.text = string.Empty;
            OnDropdownValueChanged(0);
        }

        public void OnDropdownValueChanged(int value)
        {
            switch ((PageType)value)
            {
                case PageType.ContentPage:
                    inputOptionPageTextObject.SetActive(false);
                    textOptionsObject.SetActive(false);
                    break;
                case PageType.InfoPage:
                    inputOptionPageTextObject.SetActive(true);
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

        public override void OnButtonClick()
        {
            base.OnButtonClick();
            ExperimentEditor.Instance.CreateNewPage((PageType)dropdownType.value, backgroundColor.GetColor(), textOptions.GetTextValues(), "", inputOptionPageText.text);
        }
    }
}

