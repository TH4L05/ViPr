/// <author>Thomas Krahl</author>

using UnityEngine;

namespace eccon_lab.vipr.experiment
{
    public enum PageType
    {

    }

    public enum QuestionType
    {
        RadioButton,
        InputField,
        Slider
    }

    [System.Serializable]
    public class ExperimentElement
    {
        [SerializeField] protected string name;
        [SerializeField] protected string id;
        [SerializeField] protected GameObject assigendUiElement;

        public string Id => id;
        public string Name => name;

        public virtual void Initialize(string name, string id, GameObject assignedElement)
        {
            this.name = name;
            this.id = id;
            assigendUiElement = assignedElement;
        }

        protected virtual void Setup()
        {

        }

        public virtual void OnDestroy()
        {
            Object.Destroy(assigendUiElement);
        }

        public GameObject GetUiElement()
        {
            return assigendUiElement;
        }

        public void ToggleAssignedElementVisibility(bool active)
        {
            assigendUiElement.SetActive(active);
        }
    }
}

