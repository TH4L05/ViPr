/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using eccon_lab.vipr.experiment.editor;

namespace eccon_lab.vipr.experiment
{
    public enum ExperimentType
    {
        QuestionaireOnly,
        VideoPlusQuestionaire,
    }

    [CreateAssetMenu(fileName = "newExperiment", menuName = "Data/Experiment")]
    public class Experiment : ScriptableObject
    {
        #region SerializedFields

        [SerializeField] private string experimentName;
        [SerializeField] private ExperimentType experimentType;
        [SerializeField] private string assignedVideoFileName;
        [SerializeField] private List<Page> pages;
        [SerializeField] private List<Question> questions;
        [SerializeField] private Color defaultPageBackgroundColor;
        [SerializeField] private TextOptions defaultTextValues;

        #endregion

        #region PublicFields

        public string ExperimentName => experimentName;
        public ExperimentType ExperimentType => experimentType;
        public Color DefaultPageBackgroundColor => defaultPageBackgroundColor;
        public TextOptions DefaultTextValues => defaultTextValues;

        public string AssignedVideoFile => assignedVideoFileName;

        #endregion

        #region Setup

        public void Initialize()
        {
            pages = new List<Page>();
            questions = new List<Question>();
            experimentName = "newExperiment";
            defaultPageBackgroundColor = new Color(0.1538461f, 0.06153846f, 0.2f);
            defaultTextValues = new TextOptions(Color.white, 30.0f);
        }

        public void Setup(string name, ExperimentType type)
        {
            experimentName = name;
            experimentType = type;
        }

        public void SetDefaults(Color pageColor, Color textColor, float TextSize)
        {
            defaultPageBackgroundColor = pageColor;
            defaultTextValues.textColor = textColor;
            defaultTextValues.textSize = TextSize;
        }

        public List<Page> GetPages()
        {
            return pages;
        }

        public List<Question> GetQuestions()
        {
            return questions;
        }

        #endregion;

        #region Page

        public void AddPage(Page page)
        {
            pages.Add(page);
            UpdatePageVisibility(page.Id);
        }

        public Page GetPage(string id)
        {
            foreach (Page page in pages)
            {
                if (page.Id == id) return page;
            }
            return null;
        }

        public Page GetPage(int index)
        {
            return pages[index];
        }

        public bool RemovePage(string id)
        {
            RemoveQuestionByReferenceId(id);
            foreach (Page page in pages)
            {
                if (page.Id == id)
                {
                   if (page.GetPageType() == PageType.InfoPage && page.Name == "StartPage")
                   {
                        ExperimentEditor.Instance.EditorUI.UpdateLogLabelText("The Start page cant be deleted");
                        return false;
                   }
                   if (pages.Count < 2)
                   {
                        ExperimentEditor.Instance.EditorUI.UpdateLogLabelText("The page cant be deleted");
                        return false;
                   }

                   Destroy(page.GetUiElement());
                   pages.Remove(page);
                   return true;
                } 
            }
            return false;
        }

        public int GetPageAmount()
        {
            int amount = 0;
            foreach  (Page page in pages)
            {
                if (page.Name == "StartPage") continue;
                amount++;
            }
            return amount;
        }

        public void UpdatePage(string id, Color color)
        {
            foreach (Page page in pages)
            {
                if (page.Id == id)
                {
                    page.SetBackgroundColor(color);
                    return;
                }
            }
        }
        
        public void UpdatePageVisibility(string referenceID)
        {
            foreach (Page page in pages)
            {
                page.ToggleAssignedElementVisibility(false);
            }

            foreach (Page page in pages)
            {
                if (page.Id == referenceID)
                {
                    page.ToggleAssignedElementVisibility(true);
                    break;
                }
            }
        }

        public void UpdatePageVisibility(int index)
        {
            foreach (Page page in pages)
            {
                page.ToggleAssignedElementVisibility(false);
            }
            pages[index].ToggleAssignedElementVisibility(true);
        }

        #endregion

        #region Question

        public void AddQuestion(Question question)
        {
            questions.Add(question);
        }

        public Question GetQuestion(string id)
        {
            foreach (Question question in questions)
            {
                if(question.Id == id) return question;
            }
            return null;
        }

        public void UpdateQuestion(string id, string questionText, TextOptions textValues, RadioOptionValue[] optionValues, SliderOptions sliderOptions)
        {
            foreach (Question question in questions)
            {
                if (question.Id == id)
                {
                    question.SetQuestionText(questionText);
                    question.SetTextValues(textValues);
                    question.SetRadioOptionValues(optionValues);
                    question.SetSliderOptions(sliderOptions);
                    question.SetupAssignedObject();
                    return;
                }
            }
        }

        public int GetQuestionAmount()
        {
            return questions.Count;
        }

        public void RemoveQuestion(string itemId)
        {
            foreach (Question question in questions)
            {
                if (question.Id == itemId)
                {
                    question.OnDestroy();
                    questions.Remove(question);
                    return;
                }
            }
        }

        public void RemoveQuestionByReferenceId(string referenceId)
        {
            foreach (Question question in questions)
            {
                if (question.AssignedPageId == referenceId)
                {
                    question.OnDestroy();
                    questions.Remove(question);
                    return;
                }
            }
        }

        #endregion

        public void SetPageButtonActions(ExperimentPlayer player)
        {
            int index = 0;  
            foreach (Page page in pages)
            {
                Button button = page.GetPageButton();
                TextMeshProUGUI buttonLabel = button.GetComponentInChildren<TextMeshProUGUI>();

                if (page == pages.First())
                {
                    button.onClick.AddListener(player.ShowNextPage);
                    buttonLabel.text = "Start";
                    continue;
                }

                if (page == pages.Last())
                {
                    button.onClick.AddListener(player.FinishExperiment);
                    buttonLabel.text = "Beenden";
                    return;
                }
                button.onClick.AddListener(player.ShowNextPage);
                buttonLabel.text = "Fortsetzen";
                index++;
            }
        }

        public ExperimentSaveData GetExperimentSaveData()
        {
            ExperimentSaveData saveData = new ExperimentSaveData();
            saveData.experimentName = experimentName;
            saveData.experimentType = experimentType;
            saveData.defaultPageColor = defaultPageBackgroundColor;
            saveData.defaultTextColor = defaultTextValues.textColor;
            saveData.defaultTextSize = defaultTextValues.textSize;
            saveData.pages = new List<ExperimentSaveDataPage>();
            saveData.questions = new List<ExperimentSaveDataQuestion>();

            foreach (var page in pages)
            {
                ExperimentSaveDataPage pageData = page.GetSaveData();
                saveData.pages.Add(pageData);
            }

            foreach (var question in questions)
            {
                ExperimentSaveDataQuestion questionData = question.GetSaveData();
                saveData.questions.Add(questionData);
            }
            return saveData;
        }
    }
}

