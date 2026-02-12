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
            InfoPage,
            Question,
        }

        #region SerializedFields

        [Header("Main")]
        [SerializeField] private string referenceID;
        [SerializeField] private GameObject contentObject;
        [SerializeField] private GameObject contentPrefab;
        [SerializeField] private TextMeshProUGUI nameTextField;
        [SerializeField] private List<EditorHierachyItem> contentItems = new List<EditorHierachyItem>();

        [Header("Settings")]
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color selectedColor = Color.grey;
        [SerializeField] private Color hoverColor;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image contentBackgroundImage;
        [SerializeField] private Image toggleImage;
        [SerializeField] private Sprite untoggled;
        [SerializeField] private Sprite toggled;

        #endregion

        #region PrivateFields

        private EditorHierachy editorHierachy;
        private ItemType itemType = ItemType.Invalid;
        private bool isSelected = false;
        private bool isToggled;
        private bool toggleEnabled = true;
        private RectTransform rectTransform;
        private RectTransform contentTransform;
        private RectTransform contentRootTransform;
        private float defaultHeight = 55.0f;
        private float toggledHeight = 0f;

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
            

            if (nameTextField != null) nameTextField.text = name;
            editorHierachy = hierachy;
            if (backgroundImage != null) backgroundImage.color = defaultColor;

            switch (itemType)
            {
                case ItemType.Invalid:
                    toggleEnabled = false;
                    break;
                case ItemType.Page:
                    toggleEnabled = true;
                    if (toggleImage != null)
                    {
                        toggleImage.sprite = untoggled;
                    }
                    break;
                case ItemType.InfoPage:
                    toggleEnabled = false;
                    if(toggleImage != null) toggleImage.gameObject.SetActive(false);
                    break;
                case ItemType.Question:
                    toggleEnabled = false;
                    break;
                default:
                    break;
            }

            if (contentPrefab != null)
            {
                contentObject = Instantiate(contentPrefab, transform.parent);
                contentObject.SetActive(false);
                contentObject.name = name + "_Content";
                contentBackgroundImage = contentObject.GetComponentInChildren<Image>();
                contentRootTransform = contentObject.GetComponent<RectTransform>();
                contentTransform = contentObject.transform.GetChild(1).GetComponent<RectTransform>();
                defaultHeight = 5.0f;
                toggledHeight = defaultHeight;
                if (contentBackgroundImage != null) contentBackgroundImage.color = defaultColor;
            }
        }

        #region Add/Remove Content

        public void AddContent(EditorHierachyItem item)
        {
            contentItems.Add(item);
            item.transform.SetParent(contentTransform, false);
            toggledHeight += 55f;
            ToggleContentSelect(isSelected);
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
            if (!toggleEnabled) return;
            isToggled = !isToggled;
            UpdateContent();
        }

        public void ToggleContent(bool toggle)
        {
            if (!toggleEnabled) return;
            isToggled = toggle;
            UpdateContent();
        }

        private void UpdateContent()
        {
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
                contentRootTransform.sizeDelta = new Vector2(contentRootTransform.sizeDelta.x, toggledHeight);
            }
            else
            {
                contentRootTransform.sizeDelta = new Vector2(contentRootTransform.sizeDelta.x, defaultHeight);
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
            ToggleContentSelect(isSelected);
        }

        public void EditItem()
        {
            ExperimentEditor.Instance.EditItem(referenceID, itemType);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            string id = referenceID;
            if(isSelected) return;
            ExperimentEditor.Instance.OnHierarchyItemClick(id, itemType);
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
            //if (contentBackgroundImage != null) contentBackgroundImage.color = hoverColor;
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

        public void OnItemDestroy()
        {
            if (contentItems.Count > 0)
            {
                foreach (var item in contentItems)
                {
                   item.OnItemDestroy();
                }
                contentItems.Clear();
            }

            if(contentObject != null) Destroy(contentObject);
        }
    }
}

