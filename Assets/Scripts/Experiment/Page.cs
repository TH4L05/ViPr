/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment
{
    [System.Serializable]
    public class Page : ExperimentElement
    {
        #region SerializedFields

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
        }

        #endregion

        #region Color

        public Color GetBackgroundColor()
        {
            return backgroundColor;
        }

        public void SetBackgroundColor(Color color)
        {
            backgroundColor = color;
            backgroundImage.color = backgroundColor;
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

        public void SetUiElementTransform(Transform transform)
        {
           assigendUiElement.transform.position = transform.position;
           assigendUiElement.transform.rotation = transform.rotation;
           assigendUiElement.transform.localScale = transform.localScale;
        }

        #endregion

        public ExperimentSaveDataPage GetSaveData()
        {
            ExperimentSaveDataPage pageData = new ExperimentSaveDataPage();
            pageData.pageId = id;
            pageData.pageName = name;
            pageData.backgroundColor = backgroundColor;
            return pageData;
        }
    }
}

