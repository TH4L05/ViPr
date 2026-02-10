/// <author>Thomas Krahl</author>

using UnityEngine;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class ExperimentEditorMenuWindow : MonoBehaviour
    {
        [SerializeField] private GameObject assignedGameObject;
        
        public void Start()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
        }

        public void ToggleAssignedGamebject(bool active)
        {
            if (assignedGameObject == null) return;
            assignedGameObject.SetActive(active);
        }
    }
}

