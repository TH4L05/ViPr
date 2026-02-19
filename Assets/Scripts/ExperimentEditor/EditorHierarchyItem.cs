/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class EditorHierarchyItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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
        [SerializeField] private TextMeshProUGUI nameTextField;
        [SerializeField] private GameObject contentPrefab;
        [SerializeField] private GameObject contentObject;
        [SerializeField] private List<EditorHierarchyItem> contentItems = new List<EditorHierarchyItem>();

        [Space(5f)]
        [Header("Settings")]
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color selectedColor = Color.grey;
        [SerializeField] private Color hoverColor;
        [Space(5f)]
        [SerializeField] private Image background;
        [SerializeField] private Image contentObjectBackground;
        [Space(5f)]
        [SerializeField] private Image toggleImage; 
        [SerializeField] private Sprite untoggled;
        [SerializeField] private Sprite toggled;
        [Space(5f)]
        [SerializeField] private Image selectionIndicator;
        [SerializeField] private Sprite selected;
        [SerializeField] private Sprite notSelected;

        #endregion

        #region PrivateFields

        private EditorHierarchy editorHierachy;
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

        public void Initialize(string id, string name, ItemType type, EditorHierarchy hierachy)
        {
            itemType = type;
            referenceID = id;

            isToggled = false;
            rectTransform = GetComponent<RectTransform>();
            

            if (nameTextField != null) nameTextField.text = name;
            editorHierachy = hierachy;
            if (background != null) background.color = defaultColor;

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
            ContentObjectSetup();
        }

        private void ContentObjectSetup()
        {
            if (contentPrefab == null) return;

            contentObject = Instantiate(contentPrefab, transform.parent);
            contentObject.name = name + "_Content";
            contentRootTransform = contentObject.GetComponent<RectTransform>();
            ToggleContentItem(false);
            defaultHeight = 5.0f;
            toggledHeight = defaultHeight;
            contentTransform = contentObject.transform.GetChild(1).GetComponent<RectTransform>();
            contentObjectBackground = contentObject.GetComponentInChildren<Image>();
            if (contentObjectBackground != null) contentObjectBackground.color = defaultColor;  
        }

        #region Add/Remove Content

        public void AddContent(EditorHierarchyItem item)
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
            ToggleContentItem(isToggled);
            SetHeight();
        }

        private void ToggleContentItem(bool isToggled)
        {
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
            foreach (var item in contentItems)
            {
                item.ToggleSelection(selected);
            }
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

        public void ToggleSelection(bool selected)
        {
            isSelected = selected;
            ToggleSelectionIndicator(isSelected);
            ToggleContentSelect(isSelected);

            if (isSelected)
            {
                if (background != null) background.color = selectedColor;
                if (contentObjectBackground != null) contentObjectBackground.color = selectedColor;
            }
            else
            {
                if (background != null) background.color = defaultColor;
                if (contentObjectBackground != null) contentObjectBackground.color = defaultColor;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            background.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isSelected)
            {
                if (background != null) background.color = selectedColor;
                if (contentObjectBackground != null) contentObjectBackground.color = selectedColor;
                return;
            }
            if (background != null) background.color = defaultColor;
            if (contentObjectBackground != null) contentObjectBackground.color = defaultColor;
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

            if (contentObject != null)
            {
                Destroy(contentObject);
                contentObject = null;
            }

        }

        public void ToggleSelectionIndicator(bool isSelected)
        {
            if (itemType == ItemType.Question) return;
            if (selectionIndicator == null) return;

            if (isSelected)
            {
                selectionIndicator.sprite = selected;
            }
            else
            {
                selectionIndicator.sprite = notSelected;
            }
        }
    }
}

