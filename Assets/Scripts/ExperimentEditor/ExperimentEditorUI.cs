/// <author>Thomas Krahl</author>

using System;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class ExperimentEditorUI : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] private GameObject menuWindowObject;
        [SerializeField] private GameObject startMenuObject;
        [SerializeField] private GameObject inMenuObject;
        [SerializeField] private GameObject inMenuWindowObject;
        
        [Header("MenuCreateExperiment")]
        [SerializeField] private GameObject createExperimentWindowObject;
        [SerializeField] private TMP_InputField createExperimenttextFieldName;
        [SerializeField] private TMP_Dropdown createExperimentDropdownExperimentType;
        [SerializeField] private TMP_Dropdown createExperimentDropdownAssignedVideo;
        [SerializeField] private TextMeshProUGUI createExperimentDropdownAssignedVideoLabel;
        [SerializeField] private GameObject videoSelectionElement;

        [Header("MenuLoadExperiment")]
        [SerializeField] private GameObject loadExperimentWindowObject;
        [SerializeField] private TMP_Dropdown loadExperimentDropdownFiles;

        [Header("Editor")]
        [SerializeField] private TextMeshProUGUI experimentNameLabel;
        [SerializeField] private TextMeshProUGUI logTextlabel;

        [Header("CreateQuestion")]
        [SerializeField] private GameObject createQuestionObject;
        [SerializeField] private GameObject inputOptionQuestionText;
        [SerializeField] private GameObject inputOptionRadio;
        [SerializeField] private GameObject inputOptionSlider;
        [SerializeField] private TMP_Dropdown createQuestionDropdownType;
        [SerializeField] private TMP_InputField textQuestionText;
        [SerializeField] private RadioButtonCreateOption[] radioButtonOptions;
        [SerializeField] private SliderCreateOption sliderCreateOptions;

        [Header("ElementInspector")]
        [SerializeField] private GameObject elementInspectorObject;

        [Header("ExperimentTest")]
        [SerializeField] private GameObject testUiObject;
        [SerializeField] private GameObject testUiExperimentPlayer;

        public void Initialize()
        {
            Setup();
        }

        private void Setup()
        {
            ToggleMainWindowObject(true);
            ToggleEditorMenu(true);
            SetupExperimentTypeDropdown();
            SetupQuestionTypeDropdown();
            SetupAssignedVideoDropdown();
            SetupExperimentFilesDropdown();
            SetExperimentNameLabel("-"); 

            ToggleInMenuWindowObject(false);
            ToggleElementInspectorObject(false);
            ToggleCreateQuestionObject(false);
        }

        public void ToggleMainWindowObject(bool active)
        {
            if (menuWindowObject == null) return;
            menuWindowObject.SetActive(active);
        }

        public void ToggleEditorMenu(bool active)
        {
            if (startMenuObject == null) return;
            startMenuObject.SetActive(active);
            logTextlabel.text = "";
        }

        public void ToggleEditorInMenu(bool active)
        {
            if (inMenuObject == null) return;
            inMenuObject.SetActive(active);
        }

        public void ToggleInMenuWindowObject(bool active)
        {
            if(inMenuWindowObject == null) return;
            inMenuWindowObject.SetActive(active);
        }

        #region ExperimentCreate

        public void ToogleExperimentCreateWindow(bool active)
        {
            if (createExperimentWindowObject == null) return;

            if (active)
            {
                createExperimenttextFieldName.text = "newExperiment";
                videoSelectionElement.SetActive(false);
            }
            createExperimentWindowObject.SetActive(active);
        }

        public void ToggleExperimentLoadWindow(bool active)
        {
            if(loadExperimentWindowObject == null) return;
            loadExperimentWindowObject.SetActive(active);
        }

        private void SetupExperimentTypeDropdown()
        {
            if(createExperimentDropdownExperimentType == null) return;

            foreach (var item in Enum.GetValues(typeof(ExperimentType)))
            {
                createExperimentDropdownExperimentType.options.Add(new TMP_Dropdown.OptionData(item.ToString()));
            }   
        }

        private void SetupAssignedVideoDropdown()
        {
            if(createExperimentDropdownAssignedVideo == null) return;
            FileInfo[] files = ExperimentEditor.Instance.GetFileInfosFromFolder("Videos");

            createExperimentDropdownAssignedVideo.options.Add(new TMP_Dropdown.OptionData("none"));
            foreach (FileInfo file in files)
            {
                createExperimentDropdownAssignedVideo.options.Add(new TMP_Dropdown.OptionData(file.Name));
            }
        }

        private void SetupExperimentFilesDropdown()
        {
            if (loadExperimentDropdownFiles == null) return;
            FileInfo[] files = ExperimentEditor.Instance.GetFileInfosFromFolder("Experiments");
            if (files.Length < 1)
            {
                loadExperimentDropdownFiles.options.Add(new TMP_Dropdown.OptionData("No experiments available"));
                return;
            }

            foreach (FileInfo file in files)
            {
                string extension = file.Extension;
                loadExperimentDropdownFiles.options.Add(new TMP_Dropdown.OptionData(file.Name.Replace(extension, "")));
            }
        }

        public void OnExperimentTypeDropDownChange(int value)
        {
            switch ((ExperimentType)value)
            {
                default:
                case ExperimentType.QuestionaireOnly:
                    videoSelectionElement.SetActive(false);
                    break;
                case ExperimentType.VideoPlusQuestionaire:
                    videoSelectionElement.SetActive(true);
                    break;
            }
        }

        #endregion

        #region Question

        private void SetupQuestionTypeDropdown()
        {
            if (createQuestionDropdownType == null) return;

            foreach (var item in Enum.GetValues(typeof(QuestionType)))
            {
                createQuestionDropdownType.options.Add(new TMP_Dropdown.OptionData(item.ToString()));
            }
        }

        public void ToggleCreateQuestionObject(bool active)
        {
            if (createQuestionObject == null) return;
            createQuestionObject.SetActive(active);
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

        public void OnCreateButtonClick()
        {
            Debug.Log("Create new experiment -> name = " + createExperimenttextFieldName.text);

            ExperimentType type = (ExperimentType)createExperimentDropdownExperimentType.value;
            string labelText = createExperimentDropdownAssignedVideoLabel.text;

            if (type == ExperimentType.QuestionaireOnly)
            {
                labelText = "none";
            }
            ExperimentEditor.Instance.CreateExperiment(createExperimenttextFieldName.text, (ExperimentType)createExperimentDropdownExperimentType.value, createExperimentDropdownAssignedVideoLabel.text);
        }

        public void OnLoadExperimentButtonClick()
        {
            ExperimentEditor.Instance.LoadExperiment(loadExperimentDropdownFiles.captionText.text);
        }

        public void OnCreateQuestionButtonClick()
        {
            Debug.Log("Create new question");
            ExperimentEditor.Instance.CreateNewQuestion((QuestionType)createQuestionDropdownType.value, textQuestionText.text, radioButtonOptions, sliderCreateOptions);
            ResetRadioButtonOptions();
            ToggleCreateQuestionObject(false);
        }

        #endregion

        public void ToggleElementInspectorObject(bool active)
        {
            if (elementInspectorObject == null) return; 
            elementInspectorObject.SetActive(active);
        }

        public void SetExperimentNameLabel(string name)
        {
            if (experimentNameLabel == null) return;
            experimentNameLabel.text = name;
        }

        public void ResetRadioButtonOptions()
        {
            foreach (var item in radioButtonOptions)
            {
                item.Reset();
            }
        }

        public void ToggleExperimentPlayerUi(bool active)
        {
            testUiObject.SetActive(active);
            testUiExperimentPlayer.SetActive(active);
        }

        public void UpdateLogLabelText(string text)
        {
            logTextlabel.text = text;
        }
    }
}