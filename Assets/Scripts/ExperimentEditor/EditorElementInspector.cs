/// <author>Thomas Krahl</author>

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment.editor
{ 
    public class EditorElementInspector : MonoBehaviour
    {
        [SerializeField] private GameObject elementInspectorWindow;
        [SerializeField] private TextMeshProUGUI elementNameLabel;
        [SerializeField] private EditorElementInspectorItem colorPickerItem;
        [SerializeField] private EditorElementInspectorItem textInputItem;
        [SerializeField] private EditorElementInspectorItem textOptionsItem;
        [SerializeField] private EditorElementInspectorItem radioOptionsItem;
        [SerializeField] private EditorElementInspectorItem sliderOptionsItem;
        [SerializeField] private Button deleteButton;

        private string currentId;
        private EditorHierachyItem.ItemType currentType;

        private void Start()
        {
            Setup();
            
        }

        private void Setup()
        {
            HideAllItems();
            deleteButton.onClick.AddListener(OnRemoveButtonClick);
        }
        
        public void ShowPageWindow(object obj, EditorHierachyItem.ItemType type)
        {
            elementInspectorWindow.SetActive(true);
            if (obj == null) return;
            HideAllItems();
            currentType = type;
            string name = "";

            switch (type)
            {
                case EditorHierachyItem.ItemType.Page:
                    Page p = (Page)obj;
                    DisplayPageItems(p);
                    currentId = p.Id;
                    name = p.Name;

                    elementNameLabel.text = name;
                    Debug.Log("Edit " + name);
                    ExperimentEditor.Instance.UpdateLogLabel("Edit " + name);
                    break;
                case EditorHierachyItem.ItemType.Question:
                    Question q = (Question)obj;
                    DisplayQuestionItems(q);
                    currentId = q.Id;
                    name = q.Name;
                    elementNameLabel.text = name;
                    Debug.Log("Edit " + name);
                    ExperimentEditor.Instance.UpdateLogLabel("Edit " + name);
                    break;
                default:
                    break;
            }
        }

        private void HideAllItems()
        {
            colorPickerItem.gameObject.SetActive(false);
            textInputItem.gameObject.SetActive(false);
            radioOptionsItem.gameObject.SetActive(false);
            textOptionsItem.gameObject.SetActive(false);
            sliderOptionsItem.gameObject.SetActive(false);
        }

        public void DisplayPageItems(Page page)
        {
            colorPickerItem.gameObject.SetActive(true);
            colorPickerItem.SetLabelText("Background color");
            colorPickerItem.SetColorValue(page.GetBackgroundColor());
        }
        public void DisplayQuestionItems(Question question)
        {
            colorPickerItem.gameObject.SetActive(true);
            textInputItem.gameObject.SetActive(true);
            textOptionsItem.gameObject.SetActive(true);

            switch (question.GetQuestionType())
            {
                case QuestionType.RadioButton:
                    radioOptionsItem.gameObject.SetActive(true);
                    sliderOptionsItem.gameObject.SetActive(false);
                    break;
                case QuestionType.InputField:
                    radioOptionsItem.gameObject.SetActive(false);
                    sliderOptionsItem.gameObject.SetActive(false);
                    break;
                case QuestionType.Slider:
                    radioOptionsItem.gameObject.SetActive(false);
                    sliderOptionsItem.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }

            colorPickerItem.SetLabelText("TextColor");
            colorPickerItem.SetColorValue(question.GetTextColor());
            textInputItem.SetInput(question.GetQuestionText());
            textOptionsItem.SetTextOptions(question.GetTextValues());
            radioOptionsItem.SetRadioButtonOptions(question.GetRadioOptionValues());
            sliderOptionsItem.SetSliderOptions(question.GetSliderOptionValues());
        }

        public void OnSaveButtonClick()
        {
            switch (currentType)
            {
                case EditorHierachyItem.ItemType.Page:
                    ExperimentEditor.Instance.UpdatePageValues(currentId, colorPickerItem.GetColorValue());
                    break;
                case EditorHierachyItem.ItemType.Question:
                    TextValues textValues = textOptionsItem.GetTextOptions();
                    textValues.textColor = colorPickerItem.GetColorValue();
                    ExperimentEditor.Instance.UpdateQuestionValues(currentId, textInputItem.GetInputValue(), textValues, radioOptionsItem.GetRadioOptionValues(), sliderOptionsItem.GetSliderOptions());
                    break;
                default:
                    break;
            }
            elementInspectorWindow.SetActive(false);
        }

        public void OnRemoveButtonClick()
        {
            ExperimentEditor.Instance.RemoveItem(currentId, currentType);
            currentId = string.Empty;
            elementInspectorWindow.SetActive(false);
        }

    }
}
