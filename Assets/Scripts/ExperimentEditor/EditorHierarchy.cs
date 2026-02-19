/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class EditorHierarchy : MonoBehaviour
    {
        [SerializeField] private List<EditorHierarchyItem> Items = new List<EditorHierarchyItem>();
        [SerializeField] private Transform contentRoot;

        public void AddItem(object item, EditorHierarchyItem.ItemType type, string pageReferenceId)
        {
            GameObject prefab = null;
            string name = "xxx";
            string id = "1234";
            Transform root = null;

            switch (type)
            {
                case EditorHierarchyItem.ItemType.Page:
                case EditorHierarchyItem.ItemType.InfoPage:
                    Page p = (Page)item;
                    name = p.Name;
                    id = p.Id;
                    root = contentRoot;
                    prefab = ExperimentEditor.Instance.GetPrefab("HierarchyPagePrefab");
                    root = contentRoot;
                    
                    break;
                case EditorHierarchyItem.ItemType.Question:
                    Question q = (Question)item;
                    name = q.Name;
                    id = q.Id;
                    prefab = ExperimentEditor.Instance.GetPrefab("HierarchyQuestionPrefab");
                    root = GetItemTransform(pageReferenceId);
                    
                    break;
                default:
                    break;
            }

            if (prefab == null)
            {
                Debug.LogError(" HIERARCHY PREFAB IS MISSING");
                return;
            }

            if (root == null)
            {
                Debug.LogError(" MISSING ITEM TRANSFORM");
                return;
            }

            GameObject newPageObject = Instantiate(prefab, root);
            newPageObject.name = name;
            EditorHierarchyItem newItem = newPageObject.GetComponent<EditorHierarchyItem>();
            newItem.Initialize(id, name, type, this);
            Items.Add(newItem);

            switch (type)
            {
                case EditorHierarchyItem.ItemType.Invalid:
                    break;
                case EditorHierarchyItem.ItemType.Page:
                    ToggleItemState(id);
                    break;
                case EditorHierarchyItem.ItemType.Question:
                    EditorHierarchyItem page = GetItem((item as Question).AssignedPageId);
                    page.AddContent(newItem);
                    //newItem.gameObject.SetActive(false);
                    UpdatePageToggle(pageReferenceId);
                    break;
                default:
                    break;
            }
        }

        public void RemoveItem(string referenceID)
        {
            foreach (EditorHierarchyItem item in Items)
            {
                if (item.ReferenceID == referenceID)
                {
                    if (item.Type == EditorHierarchyItem.ItemType.Question)
                    {
                        foreach (EditorHierarchyItem item2 in Items)
                        {
                            item2.RemoveContent(referenceID);
                        }
                    }
                    item.OnItemDestroy();
                    Items.Remove(item);
                    Destroy(item.gameObject);
                    return;
                }
            }
        }

        public EditorHierarchyItem GetItem(string referenceId)
        {
            foreach (EditorHierarchyItem item in Items)
            {
                if (item.ReferenceID == referenceId) return item;
            }
            return null;
        }

        public Transform GetItemTransform(string referenceId)
        {
            foreach (EditorHierarchyItem item in Items)
            {
                if (item.ReferenceID == referenceId) return item.gameObject.transform;
            }
            return null;
        }

        public void UpdatePageToggle(string pageId)
        {
            foreach (EditorHierarchyItem item in Items)
            {
                if (item.Type == EditorHierarchyItem.ItemType.Page && item.ReferenceID == pageId)
                {
                    item.ToggleContent(true);
                    return;
                }
            }
        }

        public void ToggleItemState(string id)
        {
            foreach (var item in Items)
            {
                if(item.Type == EditorHierarchyItem.ItemType.Question) continue;
                if (item.ReferenceID == id)
                {
                    item.ToggleSelection(true);
                    continue;
                }
                item.ToggleSelection(false);
            }
        }
    }
}