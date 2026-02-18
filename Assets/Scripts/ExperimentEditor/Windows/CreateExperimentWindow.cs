/// <author>Thomas Krahl</author>

using System;
using System.IO;
using TMPro;
using UnityEngine;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class CreateExperimentWindow : ExperimentEditorMenuWindow
    {
        [Space(4f)]
        [SerializeField] private TMP_InputField inputExperimentName;
        [SerializeField] private TMP_Dropdown dropdownExperimentType;
        //[SerializeField] private TMP_Dropdown dropdownAssignedVideoFile;
        [SerializeField] private TMP_InputField inputAssignedVideoFile;
        [SerializeField] private ToggleButton foldout;

        [SerializeField] private ColorPicker colorPickerBackground;
        [SerializeField] private TextOptionInspector textOptionInspector;

        public override void Initialize()
        {
            base.Initialize();
            inputExperimentName.text = "newExperiment";
            ToggleVideoFileInputObject(false);
            SetupExperimentTypeDropdown();
            //SetupAssignedVideoDropdown();
            inputAssignedVideoFile.text = string.Empty;
            foldout.Setup();
            if (colorPickerBackground != null) colorPickerBackground.Initialize();
            if (textOptionInspector != null) textOptionInspector.SetTextValues(new TextOptions(Color.white, 25.0f));
        }

        private void ToggleVideoFileInputObject(bool active)
        {
            inputAssignedVideoFile.transform.parent.gameObject.SetActive(active);
            //dropdownAssignedVideoFile.transform.parent.gameObject.SetActive(active);
        }

        private void SetupExperimentTypeDropdown()
        {
            if (dropdownExperimentType == null) return;
            foreach (var item in Enum.GetValues(typeof(ExperimentType)))
            {
                dropdownExperimentType.options.Add(new TMP_Dropdown.OptionData(item.ToString()));
            }
        }

        /*private void SetupAssignedVideoDropdown()
        {
            if (dropdownAssignedVideoFile == null) return;
            FileInfo[] files = ExperimentEditor.Instance.GetFileInfosFromFolder("Videos");

            dropdownAssignedVideoFile.options.Add(new TMP_Dropdown.OptionData("none"));
            foreach (FileInfo file in files)
            {
                dropdownAssignedVideoFile.options.Add(new TMP_Dropdown.OptionData(file.Name));
            }
        }*/

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

            string experimentName = inputExperimentName.text;
            if (string.IsNullOrEmpty(experimentName))
            {
                experimentName = "newExperiment" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            }

            string videoFileName = inputAssignedVideoFile.text;

            if (type == ExperimentType.QuestionaireOnly || string.IsNullOrEmpty(videoFileName))
            {
                videoFileName = "none";
            }
            ExperimentEditor.Instance.CreateExperiment(experimentName, type, colorPickerBackground.GetColor(), textOptionInspector.GetTextValues(), videoFileName);
        }
    }
}
