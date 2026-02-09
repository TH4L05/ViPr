/// <author>Thomas Krahl</author>

using System;
using System.IO;
using TMPro;
using UnityEngine;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class CreateExperimentWindow : MonoBehaviour
    {
        public GameObject assignedGameobjct;
        [Space(2f)]
        [SerializeField] private TMP_InputField inputExperimentName;
        [SerializeField] private TMP_Dropdown dropdownExperimentType;
        [SerializeField] private TMP_Dropdown dropdownAssignedVideoFile;
        [SerializeField] private ColorPicker colorPageBackgroundDefault;
        [SerializeField] private ColorPicker colorTextDefault;
        [SerializeField] private CustomSlider inputTextSizeDefault;
        [SerializeField] private ToggleButton toggleButton;

        public void Start()
        {
            Setup();
        }

        public void Setup()
        {
            inputExperimentName.text = "newExperiment";
            ToggleVideoFileInputObject(false);
            SetupExperimentTypeDropdown();
            SetupAssignedVideoDropdown();
            toggleButton.Setup();
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

        public void OnCreateButtonClick()
        {
            Debug.Log("Create new experiment -> name = " + inputExperimentName.text);
            ExperimentType type = (ExperimentType)dropdownExperimentType.value;
            string videoFileName = dropdownAssignedVideoFile.itemText.text;

            if (type == ExperimentType.QuestionaireOnly)
            {
                videoFileName = "none";
            }
            ExperimentEditor.Instance.CreateExperiment(inputExperimentName.text, type, colorPageBackgroundDefault.GetColor(), colorTextDefault.GetColor(), inputTextSizeDefault.GetSliderValue(), videoFileName);
        }
    }
}
