/// <author>Thomas Krahl</author>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using TK.Util;
using eccon_lab.vipr.experiment.editor.ui;

namespace eccon_lab.vipr.experiment.editor
{

    public class ExperimentEditor : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField] private string defaultSaveDirectory;

        [Header("References")]
        [SerializeField] private ExperimentEditorUI editorUI;
        [SerializeField] private EditorHierachy editorHierarchy;
        [SerializeField] private EditorElementInspector elementInspector;
        [SerializeField] private Experiment experiment;
        [SerializeField] private ExperimentPlayer experimentPlayer;
        

        [Space(4),Header("Prefabs")]
        [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
        [SerializeField] private Transform experimentRoot;

        #endregion

        #region PrivateFields

        private string currentPageId;

        #endregion

        #region PublicFields

        public static ExperimentEditor Instance;
        public ExperimentEditorUI EditorUI => editorUI;
        public Experiment CurrentExperiment => experiment;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            Initialize();
            ExperimentPlayer.OnExperimentStateChanged += ExperimentTestFinished;
        }

        private void OnDestroy()
        {
            if(experiment != null) Destroy(experiment);
            ExperimentPlayer.OnExperimentStateChanged -= ExperimentTestFinished;
        }

        #endregion

        #region Setup

        private void Initialize()
        {
            editorUI.Initialize();
        }

        #endregion

        #region CreateExperiment

        public void CreateExperiment(string experimentName, ExperimentType experimentType, Color defaultPageColor, Color defaultTextColor, float defaultTextSize, string assignedVideo = "none", bool isNewExperiment = true)
        {
            if (!Serialization.DirectoryExists(defaultSaveDirectory))
            {
                Serialization.CreateDirectory(defaultSaveDirectory);
            }

            experiment = ScriptableObject.CreateInstance<Experiment>();
            experiment.Initialize();
            experiment.name = experimentName;
            experiment.Setup(experimentName, experimentType);
            experiment.SetDefaults(defaultPageColor, defaultTextColor, defaultTextSize);
            editorUI.SetExperimentNameLabel(experimentName);
            if(!isNewExperiment) return;
            CreateNewPage(PageType.InfoPage, experiment.DefaultPageBackgroundColor, "StartPage", "Welcome");
            CreateNewPage(PageType.ContentPage, experiment.DefaultPageBackgroundColor);
            experiment.UpdatePageVisibility(experiment.GetPage(0).Id);
            editorUI.UpdateLogLabelText("Created new Experiment with name \"" + experimentName + "\"");
            currentPageId = experiment.GetPage(0).Id;
            experiment.UpdatePageVisibility(currentPageId);
            editorHierarchy.ToggleItemState(currentPageId);
        }

        #endregion

        #region LoadSaveExperiment

        public void LoadExperiment(string experimentName)
        {
            string fileContent = Serialization.LoadText("Experiments" + "\\" + experimentName + ".json");
            var experimentData = new ExperimentSaveData();
            experimentData = JsonUtility.FromJson<ExperimentSaveData>(fileContent);
            Debug.Log(experimentData.experimentName + " = loaded");

            CreateExperiment(experimentData.experimentName, (ExperimentType)experimentData.experimentType,experimentData.defaultPageColor, experimentData.defaultTextColor, experimentData.defaultTextSize,  experimentData.assignedVideoFile, false);

            foreach (var page in experimentData.pages)
            {
                CreatePage(page.pageId, page.pageName, page.pageType, page.pageText, page.textOptions, page.backgroundColor);
            }

            foreach (var question in experimentData.questions)
            {
                CreateQuestion(question.questionId, question.questionName, question.questionType, question.questionText, question.textOptions,  question.radioOptions, question.sliderOptions, question.referencePageId);
            }

            currentPageId = experiment.GetPage(0).Id;
            experiment.UpdatePageVisibility(currentPageId);
            editorHierarchy.ToggleItemState(currentPageId);
            editorUI.ToggleExperimentLoadWindow(false);
            editorUI.UpdateLogLabelText("The Experiment file \"" + experimentName + "\" was loaded");
        }

        public void SaveExperiment()
        {
            ExperimentSaveData saveData = experiment.GetExperimentSaveData();
            Serialization.SaveToJson(saveData, "Experiments" + "\\" + experiment.ExperimentName + ".json");
            editorUI.UpdateLogLabelText("Experiment saved to file \"" + experiment.ExperimentName + ".json\"");
        }

        #endregion

        #region Page

        public void CreateNewPage(PageType type, Color backgroundColor, string name = "", string pageText = "" )
        {
            string id = CreateId();
            int pagecount = 1 + experiment.GetPageAmount();
            string pageName = "Page" + pagecount.ToString("00");
            if(name != string.Empty) pageName = name;
            Color color = backgroundColor;
            if(backgroundColor == null) color = experiment.DefaultPageBackgroundColor;

            CreatePage(id, pageName,type, pageText, experiment.DefaultTextValues, color);
        }

        public void CreatePage(string id, string name,PageType type, string pageText,TextOptions textOptions,  Color backgroundColor)
        {
            GameObject prefab = null;
            EditorHierachyItem.ItemType itemType = EditorHierachyItem.ItemType.Invalid;

            switch (type)
            {
                case PageType.ContentPage:
                    prefab = GetPrefab("PageContentPrefab");
                    itemType = EditorHierachyItem.ItemType.Page;
                    break;
                case PageType.InfoPage:
                    prefab = GetPrefab("PageInfoPrefab");
                    itemType = EditorHierachyItem.ItemType.InfoPage;
                    break;
                default:
                    break;
            }

            if (prefab == null)
            {
                Debug.LogError(" PREFAB IS MISSING");
                return;
            }

            GameObject newPageObject = Instantiate(prefab, experimentRoot);
            newPageObject.name = name;
            Page newPage = new Page();
            newPage.Initialize(name, id, newPageObject);
            newPage.PageSetup(type, pageText);
            newPage.SetBackgroundColor(backgroundColor);
            experiment.AddPage(newPage);
            editorHierarchy.AddItem(newPage, itemType, "");
            currentPageId = id;
            Debug.Log("Create new Page, name = " + newPage.Name);
            editorUI.UpdateLogLabelText("Create new Page, name = " + newPage.Name);
            experiment.UpdatePageVisibility(id);
        }

        public void UpdatePageValues(string id, Color backgroundColor)
        {
            experiment.UpdatePage(id, backgroundColor);
        }

        #endregion

        #region Question

        public void CreateNewQuestion(QuestionType type, string questionText, RadioButtonCreateOption[] radioButtonOptions, SliderCreateOption sliderCreateOptions)
        {
            Page p = experiment.GetPage(currentPageId);

            if (p.GetPageType() == PageType.InfoPage)
            {
                editorUI.UpdateLogLabelText("CANT create question on pages of type " + PageType.InfoPage);
                return;
            }

            string id = CreateId();
            int count = 1 + experiment.GetQuestionAmount();
            string name = "Question" + count.ToString("000");

            RadioOptionValue[] radioValues = new RadioOptionValue[radioButtonOptions.Length];
            for (int i = 0; i < radioButtonOptions.Length; i++)
            {
                radioValues[i] = new RadioOptionValue();
                radioValues[i].optionName = radioButtonOptions[i].optionInputText.text;
                radioValues[i].isEnabled = radioButtonOptions[i].optionToggle.isOn;
            }

            SliderOptions sliderOptions = new SliderOptions();
            sliderOptions.minValue = float.Parse(sliderCreateOptions.sliderMinValue.text);
            sliderOptions.maxValue = float.Parse(sliderCreateOptions.sliderMaxValue.text);
            sliderOptions.defaultValue = float.Parse(sliderCreateOptions.sliderDefaultValue.text);
            sliderOptions.labelPrefix = sliderCreateOptions.sliderLabelPrefix.text;
            sliderOptions.labelSuffix = sliderCreateOptions.sliderLabelSuffix.text;
            sliderOptions.decimalPlaces = (int)sliderCreateOptions.decimalPlaces.value;

            CreateQuestion(id, name, type, questionText,experiment.DefaultTextValues,  radioValues, sliderOptions, currentPageId);
        }

        public void CreateQuestion(string id, string name, QuestionType type, string questionText,TextOptions textValues,  RadioOptionValue[] radioOptionValues, SliderOptions sliderOptions, string pageReferenceId)
        {
            GameObject prefab = null;
            switch (type)
            {
                case QuestionType.RadioButton:
                    prefab = GetPrefab("QuestionPrefabRadio");
                    break;
                case QuestionType.InputField:
                    prefab = GetPrefab("QuestionPrefabInput");
                    break;
                case QuestionType.Slider:
                    prefab = GetPrefab("QuestionPrefabSlider");
                    break;
                default:
                    break;
            }

            if (prefab == null)
            {
                Debug.LogError(" PREFAB IS MISSING");
                editorUI.UpdateLogLabelText(" PREFAB IS MISSING ");
                return;
            }

            Transform pageRoot = experiment.GetPage(pageReferenceId).GetContentTransform();
            GameObject newObject = Instantiate(prefab, pageRoot);
            newObject.name = name;
            Question newQuestion = new Question();
            newQuestion.Initialize( id, name, newObject);
            newQuestion.QuestionSetup(type, pageReferenceId, questionText);
            newQuestion.SetTextValues(textValues);
            newQuestion.SetRadioOptionValues(radioOptionValues);
            newQuestion.SetSliderOptions(sliderOptions);
            newQuestion.SetupAssignedObject();

            experiment.AddQuestion(newQuestion);
            editorHierarchy.AddItem(newQuestion, EditorHierachyItem.ItemType.Question, pageReferenceId);
            Debug.Log("Create new Question, name = " + newQuestion.Name);
            editorUI.UpdateLogLabelText("Create new Question, name = " + newQuestion.Name);
            return;
        }

        public void UpdateQuestionValues(string id, string questionText, TextOptions textValues, RadioOptionValue[] radioOptionValues, SliderOptions sliderOptions)
        {
            experiment.UpdateQuestion(id, questionText, textValues, radioOptionValues, sliderOptions);
        }

        #endregion

        public GameObject GetPrefab(string name)
        {
            foreach (GameObject prefab in prefabs)
            {
                if (prefab.name == name) return prefab;
            }
            return null;
        }

        public string CreateId()
        {
            var utf8 = new UTF8Encoding();
            byte[] b = utf8.GetBytes(experiment.name);
            return b[0].ToString() + b.Last().ToString() + "-" + DateTime.Now.ToString("ddMMyyyy-HHmmss-ffff");
        }

        public void RemoveItem(string referenceID, EditorHierachyItem.ItemType type)
        {
            switch (type)
            {
                case EditorHierachyItem.ItemType.Page:
                    bool success = experiment.RemovePage(referenceID);
                    if(!success) return;
                    currentPageId = experiment.GetPage(0).Id;
                    experiment.UpdatePageVisibility(currentPageId);
                    editorHierarchy.ToggleItemState(currentPageId);
                    break;
                case EditorHierachyItem.ItemType.Question:
                    experiment.RemoveQuestion(referenceID);
                    break;
                default:
                    break;
            }
            editorHierarchy.RemoveItem(referenceID);
        }

        public void EditItem(string referenceID, EditorHierachyItem.ItemType type)
        {
            object obj = null;
            switch (type)
            {
                case EditorHierachyItem.ItemType.Page:
                    Page p = experiment.GetPage(referenceID);
                    obj = p;
                    break;
                case EditorHierachyItem.ItemType.Question:
                    Question q = experiment.GetQuestion(referenceID);
                    obj = q;
                    break;
                default:
                    break;
            }
            editorUI.ToggleElementInspectorObject(true);
            elementInspector.ShowContent(obj, type);
            OnHierarchyItemClick(referenceID, type);
        }

        public void UpdateLogLabel(string text)
        {
            editorUI.UpdateLogLabelText(text);
        }

        public void OnHierarchyItemClick(string referenceID, EditorHierachyItem.ItemType type)
        {
            string id = referenceID;
            Page page = experiment.GetPage(id);
            if (page == null) return;
            experiment.UpdatePageVisibility(id);
            currentPageId = id;
            editorUI.UpdateLogLabelText(page.Name + " selected");
        }

        public FileInfo[] GetFileInfosFromFolder(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fileInfos = dir.GetFiles("*.*");
            return fileInfos;
        }

        public void TestExperiment()
        {
            editorUI.ToggleExperimentPlayerUi(true);
            SaveExperiment();
            experimentPlayer.CreateExperiment(experiment.ExperimentName);
            editorUI.UpdateLogLabelText("Experiment Test for " + experiment.ExperimentName);
        }

        public void CancelExperimentTest()
        {
            ExperimentTestFinished(ExperimentState.Canceled);
            experimentPlayer.DestroyElements();
        }

        public void ExperimentTestFinished(ExperimentState state)
        {
            editorUI.ToggleExperimentPlayerUi(false);
            editorUI.UpdateLogLabelText("Experiment Test finished");
        }
    }
}

