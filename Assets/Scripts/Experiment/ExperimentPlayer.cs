/// <author>Thomas Krahl</author>

using System;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using TK.Util;
using TMPro;
using eecon_lab.vipr.video;
using eccon_lab.vipr.experiment.editor;

namespace eccon_lab.vipr.experiment
{
    public enum ExperimentState
    {
        Invalid = -1,
        Running,
        Finished,
        Canceled = 999,

    }

    public class ExperimentPlayer : MonoBehaviour
    {
        [SerializeField] private string folderPath = "Experiments";
        [SerializeField] private GameObject[] experimentPrefabs;
        [SerializeField] private CustomVideoPlayer videoPlayer;
        [SerializeField] private bool testMode = false;
        [SerializeField] private bool saveResultsInTestMode = false;
        [SerializeField] private Experiment experiment;
        [SerializeField] private Transform experimentRootTransform;
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private PlayableDirector fadeIn;
        [SerializeField] private ExperimentState experimentState = ExperimentState.Invalid;

        public ExperimentState ExperimentState => experimentState;
        public static Action<ExperimentState> OnExperimentStateChanged;
        public static Action<int> OnExperimentPageChanged;

        private int currentPageIndex;

        public void CreateExperiment(string experimentName)
        {
            Debug.Log("Create Experiment ...");
            if (experiment == null)
            {
                experiment = ScriptableObject.CreateInstance<Experiment>();
                experiment.Initialize();
            }
            ExperimentSaveData saveData = LoadExperimentData(folderPath + "\\" +  experimentName + ".json");
            CreateExperimentElements(saveData);
        }

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

        public void CreateExperimentJson(string jsonString)
        {
            ExperimentSaveData saveData = JsonUtility.FromJson<ExperimentSaveData>(jsonString);

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

        public void FinishExperiment()
        {
            if(experimentState != ExperimentState.Canceled) experimentState = ExperimentState.Finished;
            Debug.Log("ExperimentFinished");
            DestroyElements();
            OnExperimentStateChanged?.Invoke(experimentState);
            if (!saveResultsInTestMode)
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

            foreach (var item in saveData.pages)
            {
                CreatePage(item.pageId, item.pageId, item.backgroundColor);
            }

            foreach (var item in saveData.questions)
            {
                CreateQuestion(item.questionId, item.questionName, item.questionType, item.questionText, item.textValues, item.radioOptions, item.sliderOptions, item.referencePageId);
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

        public void CreatePage(string id, string name, Color backgroundColor)
        {
            GameObject prefab = GetPrefab("PagePrefab");

            if (prefab == null)
            {
                Debug.LogError(" PREFAB IS MISSING");
                return;
            }

            GameObject newPageObject = Instantiate(prefab, experimentRootTransform);
            newPageObject.name = "Page" + name;
            Page newPage = new Page();
            newPage.Initialize(name, id, newPageObject);
            newPage.SetBackgroundColor(backgroundColor);
            experiment.AddPage(newPage);
        }

        public void CreateQuestion(string id, string name, QuestionType type, string questionText, TextValues textValues, RadioOptionValue[] radioOptionValues, SliderOptions sliderOptions, string pageReferenceId)
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

        public void SaveResults()
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

            foreach (Question question in experiment.GetQuestions())
            {
                content += question.GetAnswerValue() + ";";
            }
            Serialization.SaveText(content, resultsFilePath);
        }
    }
}

