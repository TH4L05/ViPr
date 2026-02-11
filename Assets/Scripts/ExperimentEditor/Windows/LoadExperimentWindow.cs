/// <author>Thomas Krahl</author>

using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace eccon_lab.vipr.experiment.editor.ui
{
    public class LoadExperimentWindow : ExperimentEditorMenuWindow
    {
        [Space(4f)]
        [SerializeField] private TMP_Dropdown experimentFilesDropdown;
        [SerializeField] private TextMeshProUGUI infoField;
        [SerializeField] private Button loadButton;

        public override void Initialize()
        {
            base.Initialize();
            if(infoField != null) infoField.gameObject.SetActive(false);
            SetupExperimentFilesDropdown();
        }

        private void SetupExperimentFilesDropdown()
        {
            if (experimentFilesDropdown == null) return;
            FileInfo[] files = ExperimentEditor.Instance.GetFileInfosFromFolder("Experiments");
            if (files.Length < 1)
            {
                infoField.gameObject.SetActive(true);
                loadButton.interactable = false;
                return;
            }

            foreach (FileInfo file in files)
            {
                string extension = file.Extension;
                experimentFilesDropdown.options.Add(new TMP_Dropdown.OptionData(file.Name.Replace(extension, "")));
            }
        }

        public override void OnButtonClick()
        {
            base.OnButtonClick();
            ExperimentEditor.Instance.LoadExperiment(experimentFilesDropdown.captionText.text);
        }
    }
}

