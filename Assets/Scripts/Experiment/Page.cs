/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.UI;
using eccon_lab.vipr.experiment.editor;
using JetBrains.Annotations;
using TMPro;

namespace eccon_lab.vipr.experiment
{
    [System.Serializable]
    public class Page : ExperimentElement
    {
        #region SerializedFields

        [SerializeField] private PageType pageType;
        [SerializeField] private string pageText;
        [SerializeField] private TextOptions textOptions;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color backgroundColor;
        [SerializeField] private Button pageButton;
        [SerializeField] private GameObject content;

        #endregion

        #region Initialize

        override public void Initialize(string name, string id, GameObject assignedElement)
        {
            base.Initialize(name, id, assignedElement);
            backgroundImage = assigendUiElement.GetComponentInChildren<Image>();
            pageButton = assignedElement.GetComponentInChildren<Button>();
            content = assignedElement.transform.GetChild(1).gameObject;
            textOptions = new TextOptions(Color.white, 30.0f);
        }

        public void PageSetup(PageType type, string text)
        {
            pageType = type;
            pageText = text;
            SetupAssignedObject();
        }

        #endregion

        #region Get/Set

        public PageType GetPageType()
        {
            return pageType;
        }

        public Color GetBackgroundColor()
        {
            return backgroundColor;
        }

        public void SetBackgroundColor(Color color)
        {
            backgroundColor = color;
            backgroundImage.color = backgroundColor;
        }

        public void SetPageText(string text)
        {
            pageText = text;
        }

        public string GetPageText()
        {
            return pageText;
        }

        public void SetPageTextOptions(TextOptions textOptions)
        {
            this.textOptions = textOptions;
        }

        public TextOptions GetTextOptions()
        {
            return textOptions;
        }

        #endregion

        #region UiElement

        public Transform GetContentTransform()
        {
            return content.transform;
        }

        public Button GetPageButton()
        {
            return pageButton;
        }

        #endregion

        public void SetupAssignedObject()
        {
            switch (pageType)
            {
                case PageType.ContentPage:
                    
                    break;
                case PageType.InfoPage:
                    TextMeshProUGUI textField = assigendUiElement.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
                    textField.text = pageText;
                    break;
                default:
                    break;
            }
        }

        public ExperimentSaveDataPage GetSaveData()
        {
            ExperimentSaveDataPage pageData = new ExperimentSaveDataPage(id, name, pageType, pageText, backgroundColor, textOptions);
            return pageData;
        }
    }
}

