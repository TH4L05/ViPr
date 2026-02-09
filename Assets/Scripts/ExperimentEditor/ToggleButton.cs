/// <author>Thomas Krahl</author>

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class ToggleButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject contentObject;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Image toggleImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Sprite untoggled;
        [SerializeField] private Sprite toggled;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color hoverColor;

        private bool isToggled;
        private RectTransform rootTransform;
        private RectTransform contentTransform;
        private float defaultHeight;

        private void Awake()
        {
            rootTransform = transform.parent.GetComponent<RectTransform>();
            defaultHeight = rootTransform.sizeDelta.y;
            contentTransform = contentObject.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            isToggled = false;
            if (toggleImage != null)
            {
                toggleImage.sprite = untoggled;
                backgroundImage.color = defaultColor;
            }
            if (contentObject != null) contentObject.SetActive(false);
            //if (label != null) label.color = defaultColor;
        }

        public void Setup()
        {
            rootTransform = transform.parent.GetComponent<RectTransform>();
            defaultHeight = rootTransform.sizeDelta.y;
            contentTransform = contentObject.GetComponent<RectTransform>();
            isToggled = false;
            if (toggleImage != null)
            {
                toggleImage.sprite = untoggled;
                backgroundImage.color = defaultColor;
            }
            if (contentObject != null) contentObject.SetActive(false);
            ToggleContent();
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

        public void ToggleContent()
        {
            isToggled = !isToggled;
            if (isToggled)
            {
                toggleImage.sprite = toggled;
                SetHeight();
            }
            else
            {
                toggleImage.sprite = untoggled;
                SetHeight();
            }
            if (contentObject != null) contentObject.SetActive(isToggled);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ToggleContent();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //label.color = hoverColor;
            backgroundImage.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //label.color = defaultColor;
            backgroundImage.color = defaultColor;
        }
    }
}

