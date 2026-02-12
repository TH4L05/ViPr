using System;
using System.IO;
using TMPro;
using UnityEngine;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class EditExperimentWindow : ExperimentEditorMenuWindow
    {
        [Space(4f)]
        [SerializeField] private TMP_InputField inputExperimentName;
        [SerializeField] private TMP_Dropdown dropdownExperimentType;
        [SerializeField] private TMP_Dropdown dropdownAssignedVideoFile;
        [SerializeField] private ToggleButton foldout;

        [SerializeField] private ColorPicker colorPickerBackground;
        [SerializeField] private TextOptionInspector textOptionInspector;

        public override void Initialize()
        {
            base.Initialize();
            inputExperimentName.text = "newExperiment";
            ToggleVideoFileInputObject(false);
            SetupExperimentTypeDropdown();
            SetupAssignedVideoDropdown();
            foldout.Setup();
            if (colorPickerBackground != null) colorPickerBackground.Initialize();
        }

        public override void ShowWindowContent()
        {
            Experiment ex = ExperimentEditor.Instance.CurrentExperiment;

            if (ex.ExperimentType == ExperimentType.VideoPlusQuestionaire)
            {
                ToggleVideoFileInputObject(true);
            }
            dropdownExperimentType.value = (int)ex.ExperimentType;
            colorPickerBackground.Setup(ex.DefaultPageBackgroundColor);
            textOptionInspector.SetTextValues(ex.DefaultTextValues);
        }

        private void ToggleVideoFileInputObject(bool active)
        {
            dropdownAssignedVideoFile.transform.parent.gameObject.SetActive(active);
        }

        private void SetupExperimentTypeDropdown()
        {
            if (dropdownExperimentType == null) return;
            foreach (var item in Enum.GetValues(typeof(ExperimentType)))
            {
                dropdownExperimentType.options.Add(new TMP_Dropdown.OptionData(item.ToString()));
            }
        }

        private void SetupAssignedVideoDropdown()
        {
            if (dropdownAssignedVideoFile == null) return;
            FileInfo[] files = ExperimentEditor.Instance.GetFileInfosFromFolder("Videos");

            dropdownAssignedVideoFile.options.Add(new TMP_Dropdown.OptionData("none"));
            foreach (FileInfo file in files)
            {
                dropdownAssignedVideoFile.options.Add(new TMP_Dropdown.OptionData(file.Name));
            }
        }

        public void OnExperimentTypeDropDownChange(int value)
        {
            switch ((ExperimentType)value)
            {
                default:
                case ExperimentType.QuestionaireOnly:
                    ToggleVideoFileInputObject(false);
                    break;
                case ExperimentType.VideoPlusQuestionaire:
                    ToggleVideoFileInputObject(true);
                    break;
            }
        }

        public override void OnButtonClick()
        {
            base.OnButtonClick();
            Debug.Log("Create new experiment -> name = " + inputExperimentName.text);
            ExperimentType type = (ExperimentType)dropdownExperimentType.value;
            string videoFileName = dropdownAssignedVideoFile.itemText.text;

            if (type == ExperimentType.QuestionaireOnly)
            {
                videoFileName = "none";
            }
            ExperimentEditor.Instance.UpdateExperimentData(colorPickerBackground.GetColor(), textOptionInspector.GetTextValues(), inputExperimentName.text, (ExperimentType)dropdownExperimentType.value, dropdownAssignedVideoFile.captionText.text);
        }
    }
}



