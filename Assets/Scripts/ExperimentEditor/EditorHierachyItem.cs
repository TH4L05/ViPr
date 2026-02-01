/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace eccon_lab.vipr.experiment.editor
{
    public class EditorHierachyItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public enum ItemType
        {
            Invalid = -1,
            Page,
            Question,
        }

        [SerializeField] private float defaultHeight = 50.0f;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private ToggleButton toggleButton;
        [SerializeField] private GameObject contentObject;
        [SerializeField] private List<EditorHierachyItem> contentItems = new List<EditorHierachyItem>();
        [SerializeField] private string referenceID;
        [SerializeField] private TextMeshProUGUI nameTextField;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color defaultColor = Color.dimGray;
        [SerializeField] private Color pointerEnterColor = Color.darkSlateBlue;
        [SerializeField] private Color selectedColor = Color.grey;
        [SerializeField] private GameObject subContentPrefab;
       
       
        private EditorHierachy editorHierachy;
        private RectTransform contentRectTransform;
        private RectTransform contentRectRootTransform;
        private ItemType itemType = ItemType.Invalid;
        private bool isSelected = false;
        private Image subContentBackground;
        

        public string ReferenceID => referenceID;
        public ItemType Type => itemType;
        public bool IsSelected => isSelected;

        public void Initialize(string id, string name, ItemType type, EditorHierachy hierachy)
        {
            rectTransform = GetComponent<RectTransform>();
            
            itemType = type;
            referenceID = id;
            if (nameTextField != null) nameTextField.text = name;
            editorHierachy = hierachy;
            backgroundImage.color = defaultColor;

            if (subContentPrefab == null) return;
            contentObject = Instantiate(subContentPrefab, transform.parent);
            contentObject.SetActive(false);
            if (contentObject != null)
            {
                contentRectRootTransform = contentObject.GetComponent<RectTransform>();
                contentRectTransform = contentObject.transform.GetChild(1).GetComponent<RectTransform>();
                subContentBackground = contentObject.transform.GetChild(0).GetComponent<Image>();
            }
            subContentBackground.color = defaultColor;
        }

        public void AddContent(EditorHierachyItem item)
        {
            contentItems.Add(item);
            item.transform.SetParent(contentRectTransform, false);
        }

        public void RemoveContent(string referenceId)
        {
            foreach (var item in contentItems)
            {
                if (item.referenceID == referenceId)
                {
                    contentItems.Remove(item);
                    ToggleContent();
                    ToggleContent();
                    return;
                }
            }
        }

        public void ToggleContent()
        {
            bool active = !contentObject.activeInHierarchy;

            if (active)
            {
                float size = 0f;
                foreach (var item in contentItems)
                {
                    size += 55.0f;
                }
                IncreaseHeight(size);
            }
            else
            {
                ResetHeight();
            }
            //ToggleContentItemVisibility(active);
            contentObject.SetActive(active);
            toggleButton.ToggleContent(active);
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

        private void ToggleContentItemVisibility(bool active)
        {
            contentObject.SetActive(active);
            /*foreach (var item in contentItems)
            {
                item.gameObject.SetActive(active);
            }*/
        }

        public void UnselectItem()
        {
            isSelected = false;
            backgroundImage.color = defaultColor;
            if (subContentBackground != null) subContentBackground.color = defaultColor;
            ToggleContentSelect(false);
        }

        public void ResetHeight()
        {
            Vector2 size = new Vector2(contentRectRootTransform.sizeDelta.x, defaultHeight);
            contentRectRootTransform.sizeDelta = size;
        }

        public void IncreaseHeight()
        {
            if (!isSelected) return;
            float size = 0f;
            foreach (var item in contentItems)
            {
                size += 55.0f;
            }
            ResetHeight();
            IncreaseHeight(size);
            ToggleContent();
            ToggleContent();
        }

        public void IncreaseHeight(float height)
        {
            if (height == 0f) height = defaultHeight; 
            Vector2 size = new Vector2(contentRectRootTransform.sizeDelta.x, height);
            contentRectRootTransform.sizeDelta = size;
        }

        public void EditItem()
        {
            ExperimentEditor.Instance.EditItem(referenceID, itemType);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            string id = referenceID;
            if (itemType != ItemType.Page)
            {
                id = ExperimentEditor.Instance.CurrentExperiment.GetQuestion(referenceID).AssignedPageId;
            }
            ExperimentEditor.Instance.OnHierarchyItemClick(id, itemType);
            editorHierachy.ToggleItemState(id); 
        }

        public void SetSelected()
        {
            isSelected = true;
            backgroundImage.color = selectedColor;
            if(subContentBackground != null) subContentBackground.color = selectedColor;
            ToggleContentSelect(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            backgroundImage.color = pointerEnterColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isSelected)
            {
                backgroundImage.color = selectedColor;
                if (subContentBackground != null) subContentBackground.color = selectedColor;
                return;
            }
            backgroundImage.color = defaultColor;
            if (subContentBackground != null) subContentBackground.color = defaultColor;
        }
    }
}

