/// <author>Thomas Krahl</author>

using System.IO;
using TK.Util;
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
        private string folder;

        public override void Initialize()
        {
            base.Initialize();
            if(infoField != null) infoField.gameObject.SetActive(false);
            SetupExperimentFilesDropdown();
        }

        public void SetExperimentFolder(string path)
        {
            folder = path;
        }

        private void SetupExperimentFilesDropdown()
        {
            if (experimentFilesDropdown == null) return;
            if (!Serialization.DirectoryExists(folder))
            {
                experimentFilesDropdown.transform.parent.gameObject.SetActive(false);
                infoField.gameObject.SetActive(true);
                loadButton.interactable = false;
                return;
            }

            FileInfo[] files = ExperimentEditor.Instance.GetFileInfosFromFolder("Experiments");
            if (files.Length < 1)
            {
                experimentFilesDropdown.transform.parent.gameObject.SetActive(false);
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

