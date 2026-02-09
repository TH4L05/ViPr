/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class EditorHierachyItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public enum ItemType
        {
            Invalid = -1,
            Page,
            Question,
        }

        #region SerializedFields

        [Header("Main")]
        [SerializeField] private string referenceID;
        [SerializeField] private GameObject contentObject;
        [SerializeField] private TextMeshProUGUI nameTextField;
        [SerializeField] private List<EditorHierachyItem> contentItems = new List<EditorHierachyItem>();

        [Header("Settings")]
        [SerializeField] private Color selectedColor = Color.grey;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image contentBackgroundImage;
        [SerializeField] private Image toggleImage;
        [SerializeField] private Sprite untoggled;
        [SerializeField] private Sprite toggled;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color hoverColor;

        #endregion

        #region PrivateFields

        private EditorHierachy editorHierachy;
        private ItemType itemType = ItemType.Invalid;
        private bool isSelected = false;

        private bool isToggled;
        private RectTransform rectTransform;
        private RectTransform contentTransform;
        private float defaultHeight;
        private float toggledHeight;

        #endregion

        #region PublicFields

        public string ReferenceID => referenceID;
        public ItemType Type => itemType;
        public bool IsSelected => isSelected;

        #endregion

        public void Initialize(string id, string name, ItemType type, EditorHierachy hierachy)
        {
            itemType = type;
            referenceID = id;

            isToggled = false;
            rectTransform = GetComponent<RectTransform>();
            defaultHeight = rectTransform.sizeDelta.y;

            if (nameTextField != null) nameTextField.text = name;
            editorHierachy = hierachy;
            if (backgroundImage != null) backgroundImage.color = defaultColor;

            if (toggleImage != null)
            {
                toggleImage.sprite = untoggled;
                toggledHeight = 55f;
            }

            if (contentObject != null)
            {
                contentObject.SetActive(false);
                contentTransform = contentObject.GetComponent<RectTransform>();
                if(contentBackgroundImage != null) contentBackgroundImage.color = defaultColor;
            }
        }

        #region Add/Remove Content

        public void AddContent(EditorHierachyItem item)
        {
            contentItems.Add(item);
            item.transform.SetParent(contentTransform, false);
            toggledHeight += 55f;
        }

        public void RemoveContent(string referenceId)
        {
            foreach (var item in contentItems)
            {
                if (item.referenceID == referenceId)
                {
                    contentItems.Remove(item);
                    toggledHeight -= 55f;
                    SetHeight();
                    return;
                }
            }
        }

        #endregion

        public void ToggleContent()
        {
            isToggled = !isToggled;

            if (isToggled)
            {
                if (toggleImage != null) toggleImage.sprite = toggled;
            }
            else
            {
                if (toggleImage != null) toggleImage.sprite = untoggled;
            }
            SetHeight();
            contentObject.SetActive(isToggled);
        }

        public void ToggleContent(bool toggle)
        {
            isToggled = toggle;

            if (isToggled)
            {
                if (toggleImage != null) toggleImage.sprite = toggled;
            }
            else
            {
                if (toggleImage != null) toggleImage.sprite = untoggled;
            }
            SetHeight();
            contentObject.SetActive(isToggled);
        }

        private void SetHeight()
        {
            if (isToggled)
            {
                if (toggledHeight == 0f) toggledHeight = defaultHeight;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, toggledHeight);
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, defaultHeight);
            }
        }

        public void ToggleContentSelect(bool selected)
        {
            if (selected)
            {
                foreach (var item in contentItems)
                {
                    item.SetSelected();
                }
            }
            else
            {
                foreach (var item in contentItems)
                {
                    item.UnselectItem();
                }
            }
        }

        public void UnselectItem()
        {
            isSelected = false;
            if (backgroundImage != null) backgroundImage.color = defaultColor;
            if (contentBackgroundImage != null) contentBackgroundImage.color = defaultColor;
            ToggleContentSelect(false);
        }

        public void EditItem()
        {
            ExperimentEditor.Instance.EditItem(referenceID, itemType);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            string id = referenceID;
            switch (itemType)
            {
                case ItemType.Invalid:
                    break;
                case ItemType.Page:
                    ToggleContent();
                    break;
                case ItemType.Question:
                    id = ExperimentEditor.Instance.CurrentExperiment.GetQuestion(referenceID).AssignedPageId;
                    break;
                default:
                    break;
            }
            if(isSelected) return;
            ExperimentEditor.Instance.OnHierarchyItemClick(id, itemType);
            editorHierachy.ToggleItemState(id); 
        }

        public void SetSelected()
        {
            isSelected = true;
            if (backgroundImage != null) backgroundImage.color = selectedColor;
            if (contentBackgroundImage != null) contentBackgroundImage.color = selectedColor;
            ToggleContentSelect(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            backgroundImage.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isSelected)
            {
                if (backgroundImage != null) backgroundImage.color = selectedColor;
                if (contentBackgroundImage != null) contentBackgroundImage.color = selectedColor;
                return;
            }
            if (backgroundImage != null) backgroundImage.color = defaultColor;
            if (contentBackgroundImage != null) contentBackgroundImage.color = defaultColor;
        }
    }
}

