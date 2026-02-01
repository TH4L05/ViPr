/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        [SerializeField] private TextValues defaultTextValues;

        #endregion

        #region PublicFields

        public string ExperimentName => experimentName;

        public ExperimentType ExperimentType => experimentType;

        public Color DefaultPageBackgroundColor => defaultPageBackgroundColor;
        public TextValues DefaultTextValues => defaultTextValues;

        #endregion

        #region Setup

        public void Initialize()
        {
            pages = new List<Page>();
            questions = new List<Question>();
            experimentName = "newExperiment";
            defaultPageBackgroundColor = new Color(0.1538461f, 0.06153846f, 0.2f);
            defaultTextValues = new TextValues(Color.white, 30.0f);
        }

        public void Setup(string name, ExperimentType type)
        {
            experimentName = name;
            experimentType = type;
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

        public void RemovePage(string id)
        {
            RemoveQuestionByReferenceId(id);
            foreach (Page page in pages)
            {
                if (page.Id == id)
                {
                   Destroy(page.GetUiElement());
                   pages.Remove(page);
                   return;
                } 
            }
        }

        public int GetPageAmount()
        {
            return pages.Count;
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

        public void UpdateQuestion(string id, string questionText, TextValues textValues, RadioOptionValue[] optionValues, SliderOptions sliderOptions)
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
            foreach (Page page in pages)
            {
                Button button = page.GetPageButton();
                TextMeshProUGUI buttonLabel = button.GetComponentInChildren<TextMeshProUGUI>();

                if (page == pages.Last())
                {
                    button.onClick.AddListener(player.FinishExperiment);
                    buttonLabel.text = "Beenden";
                    return;
                }
                button.onClick.AddListener(player.ShowNextPage);
                buttonLabel.text = "Fortsetzen";
            }
        }
    }
}

