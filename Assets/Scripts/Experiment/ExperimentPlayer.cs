/// <author>Thomas Krahl</author>

using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using eccon_lab.vipr.experiment.editor;
using eecon_lab.vipr.video;
using TK.Util;

namespace eccon_lab.vipr.experiment
{
    public enum ExperimentState
    {
        Invalid = -1,
        None = 0,
        Prepared = 1,
        VideoPrepared = 10,
        VideoPlaying = 11,
        VideoStopped = 12,
        VideoPaused = 13,
        VideoFinished = 15,
        Running = 20,
        Finished = 21,
        Canceled = 999,
    }

    public class ExperimentPlayer : MonoBehaviour
    {
        [SerializeField] private string folderPath = "Experiments";
        [SerializeField] private GameObject[] experimentPrefabs;
        [SerializeField] private CustomVideoPlayer videoPlayer;
        [SerializeField] private bool testMode = false;
        [SerializeField] private bool saveResults = true;
        [SerializeField] private Experiment experiment;
        [SerializeField] private Transform experimentRootTransform;
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private PlayableDirector fadeIn;
        [SerializeField] private ExperimentState experimentState = ExperimentState.Invalid;

        public ExperimentState ExperimentState => experimentState;

        public static Action<ExperimentState> OnExperimentStateChanged;
        public static Action<int> OnExperimentPageChanged;

        private int currentPageIndex;

        

       public void DropdownSetup()
       {
            if (dropdown == null) return;

            FileInfo[] files = Serialization.GetFileInfosFromFolder(folderPath);

            foreach (FileInfo file in files)
            {
                string extension = file.Extension;
                dropdown.options.Add(new TMP_Dropdown.OptionData(file.Name.Replace(extension, "")));
            }
            dropdown.value = 0;
        }

        public int GetDropdownValue()
        {
            return dropdown.value;
        }

        public string GetSaveDataFromFileByDropDownValue()
        {
            int index = GetDropdownValue();
            string fileContent = Serialization.LoadText(folderPath + "\\" + dropdown.options[index].text + ".json");
            return fileContent;
        }

        public void CreateExperiment(string experimentName)
        {
        }

        public void CreateExperiment(ExperimentSaveData saveData)
        {
            CreateTheExperiment(saveData);
        }

        public void CreateExperimentJson(string jsonString)
        {
            ExperimentSaveData saveData = JsonUtility.FromJson<ExperimentSaveData>(jsonString);
            CreateTheExperiment(saveData);  
        }

        private void CreateTheExperiment(ExperimentSaveData saveData)
        {
            Debug.Log("Create Experiment ...");
            if (fadeIn != null) fadeIn.Play();

            if (experiment == null)
            {
                experiment = ScriptableObject.CreateInstance<Experiment>();
                experiment.Initialize();
            }
            CreateExperimentElements(saveData);
        }

        public void SetPageButtonActions()
        {
            experiment.SetPageButtonActions(this);
        }

        public void StartExperiment()
        {
            currentPageIndex = 0; 
            experimentState = ExperimentState.Running;
            Debug.Log("Experiment " + experimentState);

            if (testMode)
            {
                experiment.UpdatePageVisibility(currentPageIndex);
                return;
            }

            switch (experiment.ExperimentType)
            {
                case ExperimentType.QuestionaireOnly:
                    experimentRootTransform.gameObject.SetActive(true);
                    experiment.UpdatePageVisibility(currentPageIndex);
                    break;
                case ExperimentType.VideoPlusQuestionaire:
                    videoPlayer.PrepareVideo(experiment.AssignedVideoFile);
                    break;
                default:
                    break;
            }
        }

        public void ShowNextPage()
        {
            currentPageIndex++;
            experiment.UpdatePageVisibility(currentPageIndex);
            OnExperimentPageChanged?.Invoke(currentPageIndex);
        }

        public void CancelExperiment()
        {
            experimentState = ExperimentState.Canceled;
        }

        public void FinishExperiment()
        {
            if(experimentState != ExperimentState.Canceled) experimentState = ExperimentState.Finished;
            Debug.Log("Experiment " + experimentState);
            DestroyElements();
            OnExperimentStateChanged?.Invoke(experimentState);
            if (!saveResults)
            {
                Destroy(experiment);
                return;
            }
            SaveResults();
            Destroy(experiment);
            experimentRootTransform.gameObject.SetActive(false);
        }

        public void DestroyElements()
        {
            foreach (var item in experiment.GetQuestions())
            {
                item.OnDestroy();
            }

            foreach (var item in experiment.GetPages())
            {
                item.OnDestroy();
            }
        }

        public void CreateExperimentElements(ExperimentSaveData saveData)
        {
            if (saveData.experimentName == string.Empty)
            {
                Debug.Log("No save data !!");
                return;
            }
            experiment.Setup(saveData.experimentName, saveData.experimentType);

            foreach (var page in saveData.pages)
            {
                CreatePage(page.pageId, page.pageId, page.pageType, page.pageText, page.textOptions,  page.backgroundColor);
            }

            foreach (var question in saveData.questions)
            {
                CreateQuestion(question.questionId, question.questionName, question.questionType, question.questionText, question.textOptions, question.radioOptions, question.sliderOptions, question.referencePageId);
            }
            SetPageButtonActions();
            StartExperiment();
        }

        public GameObject GetPrefab(string name)
        {
            foreach (GameObject prefab in experimentPrefabs)
            {
                if (prefab.name == name) return prefab;
            }
            return null;
        }

        public void CreatePage(string id, string name, PageType type, string pageText, TextOptions textOptions, Color backgroundColor)
        {
            GameObject prefab = null;
            switch (type)
            {
                case PageType.ContentPage:
                    prefab = GetPrefab("PageContentPrefab");
                    break;
                case PageType.InfoPage:
                    prefab = GetPrefab("PageInfoPrefab");
                    break;
                default:
                    break;
            }

            if (prefab == null)
            {
                Debug.LogError(" PREFAB IS MISSING");
                return;
            }

            GameObject newPageObject = Instantiate(prefab, experimentRootTransform);
            newPageObject.name = name;
            Page newPage = new Page();
            newPage.Initialize(name, id, newPageObject);
            newPage.PageSetup(type, pageText);
            newPage.SetBackgroundColor(backgroundColor);
            newPage.SetPageTextOptions(textOptions);
            newPage.SetupAssignedObject();
            experiment.AddPage(newPage);
        }

        public void CreateQuestion(string id, string name, QuestionType type, string questionText, TextOptions textValues, RadioOptionValue[] radioOptionValues, SliderOptions sliderOptions, string pageReferenceId)
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
                return;
            }

            Transform pageRoot = experiment.GetPage(pageReferenceId).GetContentTransform();
            GameObject newObject = Instantiate(prefab, pageRoot);
            newObject.name = name;
            Question newQuestion = new Question();
            newQuestion.Initialize(id, name, newObject);
            newQuestion.QuestionSetup(type, pageReferenceId, questionText);
            newQuestion.SetRadioOptionValues(radioOptionValues);
            newQuestion.SetSliderOptions(sliderOptions);
            newQuestion.SetTextValues(textValues);
            newQuestion.SetupAssignedObject();
            experiment.AddQuestion(newQuestion);
            return;
        }

        public ExperimentSaveData LoadExperimentData(string path)
        {
            Debug.Log("load Experimentdata from File ...");
            string fileContent = Serialization.LoadText(path);
            var experimentData = new ExperimentSaveData();
            experimentData = JsonUtility.FromJson<ExperimentSaveData>(fileContent);
            Debug.Log(experimentData.experimentName + " = loaded");
            return experimentData;
        }

        public string SaveResults()
        {
            string baseDirectoryPath = folderPath + "/Results";

            if (!Serialization.DirectoryExists(baseDirectoryPath))
            {
                Serialization.CreateDirectory(baseDirectoryPath);
            }

            string experimentDirPath = baseDirectoryPath + "/" + experiment.ExperimentName;
            if (!Serialization.DirectoryExists(experimentDirPath))
            {
                Serialization.CreateDirectory(experimentDirPath);
            }

            string resultsFilePath = experimentDirPath + "/" + "Results" + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".csv";

            string content = "";
            var questionList = experiment.GetQuestions();

            foreach (Question question in questionList)
            {
                content += question.GetAnswerValue();
                if (question != questionList.Last())
                {
                    content += ";";
                }
            }
            Serialization.SaveText(content, resultsFilePath);
            return content;
        }
    }
}

