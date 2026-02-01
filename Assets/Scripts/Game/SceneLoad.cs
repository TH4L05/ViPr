/// <author>Thomas Krahl</author>

using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace TK.SceneLoading
{
    public class SceneLoad : MonoBehaviour
    {
        [SerializeField] private PlayableDirector director;
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Slider loadSlider;
        [SerializeField] private TextMeshProUGUI loadTextField;
        [SerializeField] private float directorPlayDelayTime = 1.0f;

        private int sceneIndex = -1;
        private bool loadingStarted = false;

        private void Awake()
        {
            loadingStarted = false;
        }

        public void PlayDirectorLoadSpecificScene(int _sceneIndex)
        {
            SetSceneIndex(_sceneIndex);
            PlayDirectorLoadScene();
        }

        public void PlayDirectorLoadScene()
        {
            if (sceneIndex < 0 || sceneIndex > SceneManager.sceneCountInBuildSettings) return;

            if (!loadingStarted)
            {
                loadingStarted = true;
                if (sceneIndex < 0 || sceneIndex > SceneManager.sceneCountInBuildSettings) sceneIndex = 0;
                if (director == null)
                {
                    LoadScene();
                    return;
                }
                director.Play();
            }
        }

        public void PlayDirectorLoadSceneDelayedStart()
        {
            StartCoroutine(DelayedDirectorStart());
        }

        private IEnumerator DelayedDirectorStart()
        {
            yield return new WaitForSeconds(directorPlayDelayTime); 
            PlayDirectorLoadScene();
        }

        public void LoadScene()
        {
            if (sceneIndex < 0) return;
            //Debug.Log("load scene : " + SceneManager.GetSceneAt(sceneIndex).name);
            SceneManager.LoadScene(sceneIndex);          
        }

        public void LoadSceneAsync()
        {
            if (sceneIndex < 0) return;
            //Debug.Log("load scene : " + SceneManager.GetSceneAt(sceneIndex).name);
            StartCoroutine(LoadAsynchron(sceneIndex));
            
        }

        public void LoadSpecificScene(int sceneIndex)
        {
            if (sceneIndex < 0) return;    
            SetSceneIndex(sceneIndex);
            //Debug.Log("load scene : " + SceneManager.GetSceneAt(this.sceneIndex).name);
            SceneManager.LoadScene(this.sceneIndex);
        }

        public void LoadSpecificScene(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            //Debug.Log("load scene : " + name);
            SceneManager.LoadScene(name);
        }

        public void LoadSceneAsync(int sceneIndex)
        {
            if (sceneIndex < 0) return;
            SetSceneIndex(sceneIndex);
            StartCoroutine(LoadAsynchron(this.sceneIndex));
        }

        IEnumerator LoadAsynchron(int index)
        {
            AsyncOperation load = SceneManager.LoadSceneAsync(index);
            if(loadingScreen) loadingScreen.SetActive(true);

            while (!load.isDone)
            {
                float progress = Mathf.Clamp01(load.progress / 0.9f);
                if(loadTextField != null) loadTextField.text = Mathf.Ceil((100 * progress)).ToString() + "%";
                if(loadSlider != null) loadSlider.value = progress;
                yield return null;
            }
        }

        public void SetSceneIndex(int idx)
        {
            sceneIndex = idx;
        }

        public void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

