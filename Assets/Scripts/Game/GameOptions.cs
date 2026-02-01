/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

namespace eecon_lab.Main.Configuration
{
    public class GameOptions
    {
        #region Fields

        private GameConfig gameConfig;
        private AudioMixer audioMixer;
        private Resolution[] resolutions;
        private Camera[] cameras;

        #endregion

        #region Setup

        public void SetConfig(GameConfig cfg)
        {
            gameConfig = cfg;
        }

        public GameConfig GetConfig()
        {
            return gameConfig;
        }

        public void SetSceneCameras(Camera[] cams)
        {
            cameras = cams;
        }

        public void SetAudioMixer(AudioMixer mixer)
        {
            audioMixer = mixer;
        }

        public void Setup()
        {
            if (gameConfig.DoAudioSetup) AudioSetup();
            if (gameConfig.DoVideoSetup) VideoSetup();
        }

        public void Initialize(GameConfig cfg, AudioMixer mixer)
        {
            Debug.Log("<color=#579EBF>Initialize GameOptions</color>");
            if (cfg == null)
            {
                Debug.LogError("Gameconfig is missing !!");
                return;
            }

            if(mixer != null) SetAudioMixer(mixer);
            SetConfig(cfg);
            bool success = gameConfig.Start();
            if (success)
            {
                Setup();
                return;
            }
            Debug.Log("<color=#BF5964>Initializing gameOptions failed</color>");
        }
        
        public void AudioSetup()
        {
            if (!gameConfig.DoAudioSetup) return;
            Debug.Log("<color=#579EBF>Audio setup ...</color>");
            SetMasterVolume();
        }

        public void VideoSetup()
        {
            resolutions = Screen.resolutions;
            if (!gameConfig.DoVideoSetup) return;
            Debug.Log("<color=#579EBF>Video setup ...</color>");
            SetFullScreen();
            SetResolution();
            SetVsync();
            SetFrameRateLimit();
        }

        public void GraphicSetup()
        {
            if (!gameConfig.DoGraphicSetup) return;
            Debug.Log("<color=#579EBF>Graphic setup ...</color>");
            SetAntiAliasing();
        }

        public void OnDestroy()
        {
            gameConfig.loadDone = false;
        }

        #endregion

        #region SetValues

        public void SetVsync()
        {
            if (gameConfig.VSync)
            {
                QualitySettings.vSyncCount = 1;
            }
            else 
            {
                QualitySettings.vSyncCount = 0;
            }       
        }

        public void SetFullScreen()
        {
            Screen.fullScreen = gameConfig.FullScreen;
        }

        public void SetFullScreenMode()
        {
            Screen.fullScreenMode = gameConfig.FullScreenMode;
        }

        public void SetResolution()
        {
            if (CanUseResolution(gameConfig.ResolutionWidth, gameConfig.ResolutionHeight))
            {
                Screen.SetResolution(gameConfig.ResolutionWidth, gameConfig.ResolutionHeight, gameConfig.FullScreen);
            }
            else
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, gameConfig.FullScreen);
            }

            Debug.Log("<color=#579EBF>ScreenResolution set to ->" + Screen.width + "x" + Screen.height + "</color>");
        }

        private void SetAntiAliasing()
        {
            if (gameConfig.MsaaQuality != MsaaQuality.Disabled)
            {
                SetMsaa();
                return;
            }

            var renderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
            renderPipelineAsset.msaaSampleCount = (int)MsaaQuality.Disabled;
            if (cameras.Length < 1)
            {
                if (Camera.main == null) return;
                UniversalAdditionalCameraData camData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
                camData.antialiasing = gameConfig.AntialiasingMode;
                camData.antialiasingQuality = gameConfig.AntialiasingQuality;
                return;
            }

            foreach (Camera cam in cameras)
            {
                UniversalAdditionalCameraData camData = cam.GetComponent<UniversalAdditionalCameraData>();
                camData.antialiasing = gameConfig.AntialiasingMode;
                camData.antialiasingQuality = gameConfig.AntialiasingQuality;
            }
            Debug.Log("<color=#579EBF>SET - AntiAliasing - mode = " + gameConfig.AntialiasingMode + "</color>");
        }

        private void SetMsaa()
        {
            var renderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
            renderPipelineAsset.msaaSampleCount = (int)gameConfig.MsaaQuality;

            if (cameras.Length < 1)
            {
                if(Camera.main == null) return;
                UniversalAdditionalCameraData camData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
                camData.antialiasing = AntialiasingMode.None;
                return;
            }
            else
            {
                foreach (Camera cam in cameras)
                {
                    UniversalAdditionalCameraData camData = cam.GetComponent<UniversalAdditionalCameraData>();
                    camData.antialiasing = AntialiasingMode.None;
                }
            }
            Debug.Log("<color=#579EBF>SET - MSAA - samples = " + renderPipelineAsset.msaaSampleCount + "</color>");
            gameConfig.SetAntiAntialiasingMode(AntialiasingMode.None);
        }

        private void SetMasterVolume()
        {
            float volume = 20f * Mathf.Log10(gameConfig.MasterVolume);
            audioMixer.SetFloat("VideoVolume", volume);
        }

        public void SetFrameRateLimit()
        {
            bool active = gameConfig.FramerateLimitActive;
            int limit = gameConfig.FramerateLimit;

            if (active)
            {
                Application.targetFrameRate = limit;
            }
            else
            {
                Application.targetFrameRate = -1;
            }
        }

        #endregion

        #region ChangeValues

        public void ChangeMasterVolume(float volumeValue)
        {
            gameConfig.SetMasterVolume(volumeValue);
            SetMasterVolume();
        }

        public void IncreaseMasterVolume(float amount = 0.05f)
        {
            float volume = gameConfig.MasterVolume + amount;
            if (volume > 1) volume = 1;
            ChangeMasterVolume(volume);
        }

        public void DecreaseMasterVolume(float amount = 0.05f)
        {
            float volume = gameConfig.MasterVolume - amount;
            if (volume < 0f) volume = 0f;
            ChangeMasterVolume(volume);
        }

        #endregion

        private bool CanUseResolution(int width, int height)
        {
            foreach (Resolution resolution in resolutions)
            {
                if (resolution.width == width && resolution.height == height) return true;
            }
            return false;
        }

    }
}

