/// <author>Thomas Krahl</author>

using System.IO;
using UnityEngine;
using TMPro;

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
        [SerializeField] private CreateExperimentWindow createExperimentWindow;

        [Header("MenuLoadExperiment")]
        [SerializeField] private GameObject loadExperimentWindowObject;
        [SerializeField] private TMP_Dropdown loadExperimentDropdownFiles;

        [Header("Editor")]
        [SerializeField] private TextMeshProUGUI experimentNameLabel;
        [SerializeField] private TextMeshProUGUI logTextlabel;

        [Header("CreateQuestion")]
        [SerializeField] private CreateQuestionWindow createQuestionWindow;
        

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
            Debug.Log("Editor ui setup");
            
            SetupExperimentFilesDropdown();
            SetExperimentNameLabel("-"); 

            ToogleExperimentCreateWindow(false);
            ToggleExperimentLoadWindow(false);
            ToggleInMenuWindowObject(false);
            ToggleElementInspectorObject(false);
            ToggleCreateQuestionObject(false);

            ToggleEditorMenu(true);
        }

        public void ToggleMainWindowObject(bool active)
        {
            if (menuWindowObject == null) return;
            menuWindowObject.SetActive(active);
        }

        public void ToggleEditorMenu(bool active)
        {
            if (startMenuObject == null) return;
            ToggleMainWindowObject(active);
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
            if (createExperimentWindow.assignedGameobjct == null) return;
            ToggleMainWindowObject(active);
            createExperimentWindow.assignedGameobjct.SetActive(active);
        }

        public void ToggleExperimentLoadWindow(bool active)
        {
            if(loadExperimentWindowObject == null) return;
            ToggleMainWindowObject(active);
            loadExperimentWindowObject.SetActive(active);
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

        #endregion

        #region Question

        

        public void ToggleCreateQuestionObject(bool active)
        {
            if (createQuestionWindow.assignedGameobjct == null) return;
            ToggleInMenuWindowObject(active);
            createQuestionWindow.assignedGameobjct.SetActive(active);
        }

        

        public void OnLoadExperimentButtonClick()
        {
            ExperimentEditor.Instance.LoadExperiment(loadExperimentDropdownFiles.captionText.text);
        }

        #endregion

        public void ToggleElementInspectorObject(bool active)
        {
            if (elementInspectorObject == null) return;
            ToggleInMenuWindowObject(active);
            elementInspectorObject.SetActive(active);
        }

        public void SetExperimentNameLabel(string name)
        {
            if (experimentNameLabel == null) return;
            experimentNameLabel.text = name;
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