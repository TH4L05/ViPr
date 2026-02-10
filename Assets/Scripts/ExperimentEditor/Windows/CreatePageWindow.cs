using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class CreatePageWindow : ExperimentEditorMenuWindow
    {
        [SerializeField] private GameObject inputOptionQuestionText;
        [SerializeField] private TMP_Dropdown dropdownType;
        [SerializeField] private ColorPicker cp;
        //[SerializeField] private EditorElementInspectorItem textInputItem;
        //[SerializeField] private EditorElementInspectorItem textOptionsItem;

        public override void Initialize()
        {
            if(cp != null) cp.Initialize();
        }

        public void OnCreateButtonClicked()
        {
            ExperimentEditor.Instance.CreateNewPage(PageType.ContentPage, cp.GetColor());
        }
    }
}

