/// <author>Thomas Krahl</author>

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment.editor.ui
{ 
    public class EditorElementInspector : ExperimentEditorMenuWindow
    {
        [SerializeField] private TextMeshProUGUI elementNameLabel;
        [SerializeField] private EditorElementInspectorItem colorPickerItem;
        [SerializeField] private EditorElementInspectorItem textInputItem;
        [SerializeField] private EditorElementInspectorItem textOptionsItem;
        [SerializeField] private EditorElementInspectorItem radioOptionsItem;
        [SerializeField] private EditorElementInspectorItem sliderOptionsItem;
        [SerializeField] private Button deleteButton;

        private string currentId;
        private EditorHierachyItem.ItemType currentType;

        public override void Initialize()
        {
            base.Initialize();
            colorPickerItem.Initialize();
            deleteButton.onClick.AddListener(OnRemoveButtonClick);
            HideAllItems();
        }
   
        public void ShowContent(object obj, EditorHierachyItem.ItemType type)
        {
            if (obj == null || type == EditorHierachyItem.ItemType.Invalid) return;
            HideAllItems();
            string name = "";
            currentType = type;
            switch (currentType)
            {
                case EditorHierachyItem.ItemType.Page:
                case EditorHierachyItem.ItemType.InfoPage:
                    Page p = (Page)obj;
                    currentId = p.Id;
                    name = p.Name;
                    elementNameLabel.text = "Edit " + name;
                    Debug.Log("Edit " + name);
                    DisplayPageItems(p);
                    break;
                case EditorHierachyItem.ItemType.Question:
                    Question q = (Question)obj;
                    currentId = q.Id;
                    name = q.Name;
                    elementNameLabel.text = "Edit " + name;
                    Debug.Log("Edit " + name);
                    DisplayQuestionItems(q);
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
            colorPickerItem.SetColorValue(page.GetBackgroundColor());

            switch (page.GetPageType())
            {
                case PageType.ContentPage:
                    
                    break;
                case PageType.InfoPage:
                    textInputItem.gameObject.SetActive(true);
                    textInputItem.SetInput(page.GetPageText());
                    textInputItem.SetLabelText("Page text");
                    textOptionsItem.gameObject.SetActive(true);
                    textOptionsItem.SetTextOptions(page.GetTextOptions());
                    break;
                default:
                    break;
            }
        }
        public void DisplayQuestionItems(Question question)
        {
            textInputItem.gameObject.SetActive(true);
            textInputItem.SetLabelText("Question text");
            textInputItem.SetInput(question.GetQuestionText());

            textOptionsItem.gameObject.SetActive(true);
            textOptionsItem.SetTextOptions(question.GetTextValues());

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
                    radioOptionsItem.gameObject.SetActive(false);
                    sliderOptionsItem.gameObject.SetActive(false);
                    break;
            }

            
            
            
            radioOptionsItem.SetRadioButtonOptions(question.GetRadioOptionValues());
            sliderOptionsItem.SetSliderOptions(question.GetSliderOptionValues());
        }

        public override void OnButtonClick()
        {
            base.OnButtonClick();
            switch (currentType)
            {
                case EditorHierachyItem.ItemType.Page:
                case EditorHierachyItem.ItemType.InfoPage:
                    ExperimentEditor.Instance.UpdatePageValues(currentId, colorPickerItem.GetColorValue(), textOptionsItem.GetTextOptions());
                    break;
                case EditorHierachyItem.ItemType.Question:
                    ExperimentEditor.Instance.UpdateQuestionValues(currentId, textInputItem.GetInputValue(), textOptionsItem.GetTextOptions(), radioOptionsItem.GetRadioOptionValues(), sliderOptionsItem.GetSliderOptions());
                    break;
                default:
                    break;
            }
        }

        public void OnRemoveButtonClick()
        {
            ExperimentEditor.Instance.RemoveItem(currentId, currentType);
            currentId = string.Empty;
        }

    }
}
