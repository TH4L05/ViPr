using TMPro;
using UnityEngine;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class SaveExperimentWindow : ExperimentEditorMenuWindow
    {
        [SerializeField] private TMP_InputField inputFilename;

        public override void Initialize()
        {
            base.Initialize();
            if(inputFilename != null) inputFilename.text = string.Empty;
        }

        public override void ShowWindowContent()
        {
            inputFilename.text = ExperimentEditor.Instance.CurrentExperiment.ExperimentName;
        }

        public override void OnButtonClick()
        {
            base.OnButtonClick();
            string name = inputFilename.text;
            if (string.IsNullOrEmpty(name))
            {
                name = ExperimentEditor.Instance.CurrentExperiment.ExperimentName;
            }
            ExperimentEditor.Instance.SaveExperiment(name);
        }
    }
}

    
