/// <author>Thomas Krahl</author>

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class ToggleButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform rootTransform;
        [SerializeField] private GameObject contentObject;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Image toggleImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Sprite untoggled;
        [SerializeField] private Sprite toggled;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color hoverColor;

        private bool isInitialized;
        private bool isToggled;
        
        private RectTransform contentTransform;
        private float defaultHeight;

        private void OnEnable()
        {
            if (isInitialized) return;
            Setup();
        }

        public void Setup()
        {
            if(rootTransform == null) rootTransform = GetComponent<RectTransform>();
            
            isToggled = false;
            if (toggleImage != null)
            {
                toggleImage.sprite = untoggled;
                backgroundImage.color = defaultColor;
            }
            if (contentObject != null)
            {
                contentTransform = contentObject.GetComponent<RectTransform>();
                contentObject.SetActive(false);
            }
            else
            {
                contentTransform = rootTransform;
            }
            defaultHeight = rootTransform.sizeDelta.y - contentTransform.sizeDelta.y;
            SetHeight();
            isInitialized = true;
        }

        public void ToggleContent(bool active)
        {
            isToggled = active;
            if (isToggled)
            {
                toggleImage.sprite = toggled;
            }
            else
            {
                toggleImage.sprite = untoggled;
            }
            SetHeight();
            if (contentObject != null) contentObject.SetActive(isToggled);
        }

        public void ToggleContent()
        {
            isToggled = !isToggled;
            if (isToggled)
            {
                toggleImage.sprite = toggled;
            }
            else
            {
                toggleImage.sprite = untoggled;
            }
            SetHeight(); 
            if (contentObject != null) contentObject.SetActive(isToggled);
        }

        private void SetHeight()
        {

            if (isToggled)
            {
                rootTransform.sizeDelta = new Vector2(rootTransform.sizeDelta.x, defaultHeight + contentTransform.sizeDelta.y);
            }
            else
            {
                rootTransform.sizeDelta = new Vector2(rootTransform.sizeDelta.x, defaultHeight);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ToggleContent();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            backgroundImage.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            backgroundImage.color = defaultColor;
        }
    }
}

