/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;

namespace eccon_lab.vipr.experiment.editor
{
    public class EditorHierachy : MonoBehaviour
    {
        [SerializeField] private List<EditorHierachyItem> Items = new List<EditorHierachyItem>();
        [SerializeField] private Transform contentRoot;

        public void AddItem(object item, EditorHierachyItem.ItemType type, string pageReferenceId)
        {
            GameObject prefab = null;
            string name = "xxx";
            string id = "1234";
            Transform root = null;

            switch (type)
            {
                case EditorHierachyItem.ItemType.Page:
                    Page p = (Page)item;
                    name = p.Name;
                    id = p.Id;
                    root = contentRoot;
                    prefab = ExperimentEditor.Instance.GetPrefab("HierarchyPagePrefab");
                    root = contentRoot;
                    
                    break;
                case EditorHierachyItem.ItemType.Question:
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
            EditorHierachyItem newItem = newPageObject.GetComponent<EditorHierachyItem>();
            newItem.Initialize(id, name, type, this);
            Items.Add(newItem);

            switch (type)
            {
                case EditorHierachyItem.ItemType.Invalid:
                    break;
                case EditorHierachyItem.ItemType.Page:
                    ToggleItemState(id);
                    break;
                case EditorHierachyItem.ItemType.Question:
                    EditorHierachyItem page = GetItem((item as Question).AssignedPageId);
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
            foreach (EditorHierachyItem item in Items)
            {
                if (item.ReferenceID == referenceID)
                {
                    if (item.Type == EditorHierachyItem.ItemType.Question)
                    {
                        foreach (EditorHierachyItem item2 in Items)
                        {
                            item2.RemoveContent(referenceID);
                        }
                    }
                    Items.Remove(item);
                    Destroy(item.gameObject);
                    return;
                }
            }
        }

        public EditorHierachyItem GetItem(string referenceId)
        {
            foreach (EditorHierachyItem item in Items)
            {
                if (item.ReferenceID == referenceId) return item;
            }
            return null;
        }

        public Transform GetItemTransform(string referenceId)
        {
            foreach (EditorHierachyItem item in Items)
            {
                if (item.ReferenceID == referenceId) return item.gameObject.transform;
            }
            return null;
        }

        public void UpdatePageToggle(string pageId)
        {
            foreach (EditorHierachyItem item in Items)
            {
                if (item.Type == EditorHierachyItem.ItemType.Page && item.ReferenceID == pageId)
                {
                    item.IncreaseHeight();
                    return;
                }
            }
        }

        public void ToggleItemState(string id)
        {
            foreach (var item in Items)
            {
                if(item.Type == EditorHierachyItem.ItemType.Question) continue;
                if (item.ReferenceID == id)
                {
                    item.SetSelected();
                    continue;
                }
                item.UnselectItem();
            }
        }
    }
}