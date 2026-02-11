/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.Events;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class ExperimentEditorMenuWindow : MonoBehaviour
    {
        public UnityEvent OnButtonClickEvent;
        [SerializeField] private GameObject assignedGameObject;
        
        /*public void Start()
        {
            Initialize();
        }*/

        public virtual void Initialize()
        {
            Debug.Log("initialize window");
        }

        public void ToggleAssignedGamebject(bool active)
        {
            if (assignedGameObject == null) return;
            assignedGameObject.SetActive(active);
            if (active) ShowWindowContent();
        }

        public virtual void ShowWindowContent()
        {
        }

        public virtual void OnButtonClick()
        {
            OnButtonClickEvent?.Invoke();
        }
    }
}

