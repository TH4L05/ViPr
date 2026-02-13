/// <author>Thomas Krahl</author>

using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class ExperimentEditorUI : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] private GameObject menuWindow;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private Button mainMenuCloseButton;
        
        [Header("Windows")]
        [SerializeField] private CreateExperimentWindow createExperimentWindow;
        [SerializeField] private EditExperimentWindow editExperimentWindow;
        [SerializeField] private LoadExperimentWindow loadExperimentWindow;
        [SerializeField] private SaveExperimentWindow saveExperimentWindow;
        [SerializeField] private CreatePageWindow createPageWindow;
        [SerializeField] private CreateQuestionWindow createQuestionWindow;

        [Header("Editor")]
        [SerializeField] private TextMeshProUGUI experimentNameLabel;
        [SerializeField] private TextMeshProUGUI logTextlabel;

        [Header("ElementInspector")]
        [SerializeField] private GameObject elementInspectorObject;

        [Header("ExperimentTest")]
        [SerializeField] private GameObject testUiObject;
        [SerializeField] private GameObject testUiExperimentPlayer;

        public void Initialize()
        {
            Setup();
            if(createExperimentWindow != null) createExperimentWindow.Initialize();
            if(loadExperimentWindow != null) loadExperimentWindow.Initialize();
            if(saveExperimentWindow != null) saveExperimentWindow.Initialize();
        }

        private void Setup()
        {
            Debug.Log("Editor ui setup");
            SetExperimentNameLabel("-"); 
            ToggleExperimentLoadWindow(false);
            ToogleExperimentCreateWindow(false);
            ToogleExperimentEditWindow(false);
            ToggleElementInspectorObject(false);
            ToggleCreatepageObject(false);
            ToggleCreateQuestionObject(false);
            ToggleEditorMenu(true);
            ToggleMainMenuState(true);
        }

        public void InitWindows()
        {
            if (createPageWindow != null) createPageWindow.Initialize();
            if (createQuestionWindow != null) createQuestionWindow.Initialize();
            if (editExperimentWindow != null) editExperimentWindow.Initialize();
        }

        public void ToggleMainWindowObject(bool active)
        {
            if (menuWindow == null) return;
            menuWindow.SetActive(active);
        }

        public void ToggleEditorMenu(bool active)
        {
            if (mainMenu == null) return;
            ToggleMainWindowObject(active);
            mainMenu.SetActive(active);
            logTextlabel.text = "";
        }

        public void ToggleMainMenuState(bool start)
        {
            mainMenuCloseButton.interactable = !start;
        }

        public void ToogleExperimentCreateWindow(bool active)
        {
            if (createExperimentWindow == null) return;
            ToggleMainWindowObject(active);
            createExperimentWindow.ToggleAssignedGamebject(active);
        }

        public void ToogleExperimentEditWindow(bool active)
        {
            if (editExperimentWindow == null) return;
            ToggleMainWindowObject(active);
            editExperimentWindow.ToggleAssignedGamebject(active);
        }

        public void ToggleExperimentLoadWindow(bool active)
        {
            if(loadExperimentWindow == null) return;
            ToggleMainWindowObject(active);
            loadExperimentWindow.ToggleAssignedGamebject(active);
        }

        public void ToggleExperimentSaveWindow(bool active)
        {
            if (saveExperimentWindow == null) return;
            ToggleMainWindowObject(active);
            saveExperimentWindow.ToggleAssignedGamebject(active);
        }

        public void ToggleCreatepageObject(bool active)
        {
            if (createPageWindow == null) return;
            ToggleMainWindowObject(active);
            createPageWindow.ToggleAssignedGamebject(active);
        }

        public void ToggleCreateQuestionObject(bool active)
        {
            if (createQuestionWindow == null) return;
            ToggleMainWindowObject(active);
            createQuestionWindow.ToggleAssignedGamebject(active);
        }

        public void ToggleElementInspectorObject(bool active)
        {
            if (elementInspectorObject == null) return;
            ToggleMainWindowObject(active);
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