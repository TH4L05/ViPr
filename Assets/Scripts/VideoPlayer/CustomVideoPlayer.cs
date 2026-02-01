/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using eecon_lab.Main;

namespace eecon_lab.vipr.video
{
    public class CustomVideoPlayer : MonoBehaviour
    {
        public enum VideoPlayerState
        {
            none = -1,
            playing,
            paused,
            stopped,
            finished
        }

        #region SerializedFields

        [Header("Settings")]
        [SerializeField] private string videoFolderName = "Videos";
        [SerializeField] private bool loopVideos = false;

        [Header("Ref")]
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private GameObject[] menuObjects = new GameObject[0];
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private GameObject playerMain;
        [SerializeField] private GameObject playerControls;
        [SerializeField] private Material playerMaterial;

        [Header("Playables")]
        [SerializeField] private PlayableDirector directorVideoStart;
        [SerializeField] private PlayableDirector directorVideoStop;
        [SerializeField] private PlayableDirector directorVideoLoopPointReached;

        [Header("UI")]
        [SerializeField] private TMP_Dropdown videoSelectionDropdown;
        [SerializeField] private Button playButton;

        [Header("UI Controls")]
        [SerializeField] private Slider videoSlider;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TextMeshProUGUI videoControlsTimeCurrentTextField;
        [SerializeField] private TextMeshProUGUI videoControlsTimeTotalTextField;
        [SerializeField] private ButtonImageToggle pauseButtonToggle;
        [SerializeField] private TextMeshProUGUI playbackSpeedTextField;
        [SerializeField] private TextMeshProUGUI volumeTextField;

        #endregion

        #region PrivateFields

        private int seconds = 0;
        private int minutes = 0;
        private bool sliderSelected;
        private Dictionary<int,string> videoFiles = new Dictionary<int,string>();
        private Vector2 videoResoultion;
        private float playbackSpeed = 1.0f;
        private Vector2[] resolutions = {
                                            new Vector2(1, 1),
                                            new Vector2(2, 2),
                                            new Vector2(5120, 5120),
                                            new Vector2(4096, 4096),
                                            new Vector2(3840, 3840),
                                            new Vector2(2880, 2880),
                                            new Vector2(1920, 1920),
                                         };
        private bool hidePlayerMainUiPermanent;

        #endregion

        #region UnityFunctions

        private void Start()
        {
            Setup();
        }

        private void Update()
        {
            if (videoPlayer != null && videoPlayer.isPlaying)
            {
                UpdatePlayerTime();
            }
        }

        private void OnDestroy()
        {
            videoPlayer.loopPointReached -= OnLoopPointReched;
            videoPlayer.prepareCompleted -= StartVideo;
        }

        #endregion

        #region Setup

        private void Setup()
        {
            LoadFromFolder(videoFolderName);

            if(videoPlayer == null) return;
            playerControls.SetActive(false);
            ChangeVolume(Game.Instance.GameOptions.GetConfig().MasterVolume);
            ChangePlaybackSpeed(2);
            
            videoSlider.onValueChanged.AddListener(SliderHandleUpdate);
            videoPlayer.loopPointReached += OnLoopPointReched;
            videoPlayer.prepareCompleted += StartVideo;
        }

        private void LoadFromFolder(string path)
        {
            string pathFull = path;

            if (Directory.Exists(pathFull))
            {
                DirectoryInfo dir = new DirectoryInfo(pathFull);
                FileInfo[] fileInfos = dir.GetFiles("*.*");

                if (fileInfos.Length < 1)
                {
                    videoSelectionDropdown.options.Add(new TMP_Dropdown.OptionData("No Videos found !!!"));
                    videoSelectionDropdown.value = 0;
                    videoSelectionDropdown.RefreshShownValue();
                    playButton.enabled = false;
                    return;
                }

                int count = 0;
                foreach (FileInfo fileInfo in fileInfos)
                {
                    videoFiles.Add(count, fileInfo.FullName);
                    count++;
                    videoSelectionDropdown.options.Add(new TMP_Dropdown.OptionData(fileInfo.Name, null, Color.white));
                }
                videoSelectionDropdown.value = 0;
                videoSelectionDropdown.RefreshShownValue();
            }
            else
            {
                Debug.Log("Folder Does not exist");
                videoSelectionDropdown.options.Add(new TMP_Dropdown.OptionData("Video Folder not found !!!"));
                videoSelectionDropdown.value = 0;
                videoSelectionDropdown.RefreshShownValue();
                playButton.enabled = false;
            }
        }

        public void ReloadFolderContent()
        {
            LoadFromFolder(videoFolderName);
        }

        public void OnVideoSizeDropdownChanged(int value)
        {
            videoResoultion = resolutions[value];
        }

        #endregion

        #region PrepareVideo

        public void PrepareVideo(int listindex)
        {
            if (videoPlayer == null) return;
            Prepare(videoFiles[listindex]);
        }

        public void PrepareVideo()
        {
            if (videoPlayer == null) return;
            Prepare(videoFiles[videoSelectionDropdown.value]);
            
        }

        private void Prepare(string url)
        {
            videoPlayer.url = url;
            videoPlayer.Prepare();
            videoPlayer.SetTargetAudioSource(0, audioSource);
            
            minutes = 0;
            seconds = 0;
        }

        private void CreateRenderTexture(uint width, uint height)
        {
            height = width;
            if (videoResoultion == new Vector2(1, 1) || videoResoultion == Vector2.zero)
            {
                renderTexture = new RenderTexture((int)width, (int)height, 0);
            }
            else if (videoResoultion == new Vector2(2, 2))
            {
                renderTexture = new RenderTexture((int)width / 2, (int)height / 2, 0);
            }
            else
            {
                renderTexture = new RenderTexture((int)videoResoultion.x, (int)videoResoultion.y, 0);
            }

            renderTexture.name = "VideoPlayerRenderTexture";
            renderTexture.anisoLevel = 16;
            renderTexture.antiAliasing = 8;
            renderTexture.format = RenderTextureFormat.ARGB32;
            renderTexture.Create();

            videoPlayer.targetTexture = renderTexture;
            playerMaterial.mainTexture = renderTexture;
            Debug.Log("RenderTextureSize = " + renderTexture.width + "x" + renderTexture.height);
        }

        public void DestroyRenderTexture()
        {
            renderTexture.Release();
            renderTexture.DiscardContents();
            renderTexture = null;
        }

        #endregion

        #region VideoPlay

        private void StartVideo(VideoPlayer player)
        {
            CreateRenderTexture(videoPlayer.width, videoPlayer.height);
            videoSlider.maxValue = (float)videoPlayer.length;
            SetTotalTime(videoPlayer.length);
            directorVideoStart.Play();

        }

        public void PlayVideo()
        {
            if (videoPlayer == null) return;
            videoPlayer.Play();
            Level.Instance.NetworkManagement.UpdateVideoPlayerStateClient(CustomVideoPlayer.VideoPlayerState.playing);
        }

        public void PauseVideo()
        {
            if (videoPlayer == null) return;
            if (videoPlayer.isPaused)
            {
                PlayVideo();
                pauseButtonToggle.ToggleSprite(true);
                return;
            }
            Level.Instance.NetworkManagement.UpdateVideoPlayerStateClient(VideoPlayerState.paused);
            videoPlayer.Pause();
            pauseButtonToggle.ToggleSprite(false);
        }

        public void StopVideo()
        {
            if (videoPlayer == null) return;
            videoPlayer.Stop();
            directorVideoStop.Play();
            Level.Instance.NetworkManagement.UpdateVideoPlayerStateClient(VideoPlayerState.stopped);
            DestroyRenderTexture();
        }

        private void OnLoopPointReched(VideoPlayer source)
        {
            Debug.Log("Loop Point Reached");
            if (loopVideos)
            {
                videoPlayer.Play();
                return;
            }
            directorVideoLoopPointReached.Play();
            Level.Instance.NetworkManagement.UpdateVideoPlayerStateClient(VideoPlayerState.finished);
            DestroyRenderTexture();
        }
        
        public void ToggleMenuObjectsVisibility(bool active)
        {
            foreach (GameObject gameObject in menuObjects)
            {
                gameObject.SetActive(active);
            }
        }

       

        #endregion

        #region VideoPlayerControls

        public void SliderHandleUpdate(float value)
        {
            if (!sliderSelected) return;
            videoPlayer.time = value;
            UpdatePlayerTime();
        }

        public void UpdateVideoSlider()
        {
            videoSlider.value = (float)videoPlayer.time;
        }

        private void UpdatePlayerTime()
        {
            seconds = (int)videoPlayer.time % 60;
            minutes = Mathf.FloorToInt((float)videoPlayer.time / 60);
            videoControlsTimeCurrentTextField.text = minutes.ToString("00") + ":" + seconds.ToString("00");

            if(sliderSelected) return ;
            UpdateVideoSlider();
        }

        public void ToggleSliderSlection(bool selected)
        {
            sliderSelected = selected;
        }

        private void SetTotalTime(double length)
        {
            int seconds = (int)length % 60;
            int minutes = Mathf.FloorToInt((float)length / 60);
            videoControlsTimeTotalTextField.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        
        public void ChangeVolume(float value)
        {
            Game.Instance.GameOptions.ChangeMasterVolume(value);
            if(volumeTextField != null) volumeTextField.text = Mathf.FloorToInt(value * 100).ToString();
        }
        public void ChangePlaybackSpeed(float speed)
        {
            switch (speed)
            {
                case 0:
                    playbackSpeed = 0.25f;
                    break;

                case 1:
                    playbackSpeed = 0.50f;
                    break;

                case 2:
                    playbackSpeed = 1.00f;
                    break;

                case 3:
                    playbackSpeed = 1.25f;
                    break;

                case 4:
                    playbackSpeed = 1.50f;
                    break;

                case 5:
                    playbackSpeed = 2.00f;
                    break;



                default:
                    break;
            }

            if (playbackSpeedTextField != null) playbackSpeedTextField.text = playbackSpeed.ToString() + "x";
            videoPlayer.playbackSpeed = playbackSpeed;
        }

        #endregion

        #region Material

        public void UpdateMaterialImageType(int value)
        {
            switch (value)
            {

                case 0:
                    playerMaterial.SetFloat("_ImageType", 0.0f);
                    break;

                case 1:
                    playerMaterial.SetFloat("_ImageType", 1.0f);
                    break;

                default:
                    break;
            }
        }

        public void UpdateMaterialLayout(int value)
        {
            switch (value)
            {

                case 0:
                    playerMaterial.SetFloat("_Layout", 2.0f);
                    break;

                case 1:
                    playerMaterial.SetFloat("_Layout", 1.0f);
                    break;

                case 2:
                    playerMaterial.SetFloat("_Layout", 0.0f);
                    break;

                default:
                    break;
            }
        }

        #endregion

        public void ToggleSelectionUI()
        {
            if(videoPlayer.isPlaying) return;
            if(hidePlayerMainUiPermanent) return;
            ChangeSelectionUiState();
        }

        private void ChangeSelectionUiState()
        {
            bool state = playerMain.activeSelf;
            playerMain.SetActive(!state);
        }

        public void ToogleSelectionUiPermanent()
        {
            hidePlayerMainUiPermanent = !hidePlayerMainUiPermanent;
            ChangeSelectionUiState();
        }

    }  
}

