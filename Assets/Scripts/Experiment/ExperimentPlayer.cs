

using eecon_lab.vipr.video;
using TK.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using System;

namespace eccon_lab.vipr.experiment
{
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

        public static Action OnExperimentFinished;

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

        public void CreateExperiment(ExperimentSaveData saveData)
        {
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


            if (testMode)
            {
                experiment.UpdatePageVisibility(currentPageIndex);
                return;
            }

            switch (experiment.ExperimentType)
            {
                case ExperimentType.QuestionaireOnly:
                    break;
                case ExperimentType.VideoPlusQuestionaire:
                    //videoPlayer.
                    break;
                default:
                    break;
            }
        }

        public void ShowNextPage()
        {
            currentPageIndex++;
            experiment.UpdatePageVisibility(currentPageIndex);
        }

        public void FinishExperiment()
        {
            Debug.Log("ExperimentFinished");
            DestroyElements();
            OnExperimentFinished?.Invoke();
            if (testMode && !saveResultsInTestMode)
            {
                return;
            }
            SaveResults();
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
            Destroy(experiment);
        }

        public void CreateExperimentElements(ExperimentSaveData saveData)
        {
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

