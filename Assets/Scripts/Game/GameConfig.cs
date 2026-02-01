/// <author>Thomas Krahl</author>

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TK.Util;

namespace eecon_lab.Main.Configuration
{
    [CreateAssetMenu(fileName = "newGameConfig", menuName = "Data/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public enum GameLanguage
        {
            Invalid = -1,
            Ger,
            Eng,
            Esp
        }

        #region SerializedFields

        [Header("Base")]
        [SerializeField] private bool dontCreateConfigFile = false;
        [SerializeField] private bool usePersistentDataPath = true;
        [SerializeField] private string configname = "settings.cfg";

        [Header("Setup")]
        [SerializeField] private bool doAudioSetup = true;
        [SerializeField] private bool doVideoSetup = false;
        [SerializeField] private bool doGraphicSetup = false;

        [Header("Default-Main"), Space(2f)]
        [SerializeField] private bool defaultUseVR = true;
        [SerializeField] private bool defaultShowFPS = false;
        [SerializeField] private bool defaultShowLog = false;

        //[Header("Default-Extras"), Space(2f)]

        [Header("Default-Audio"), Space(2f)]
        [SerializeField, Range(0.0001f, 1.0f)] private float defaultMasterVolume = 0.65f;

        [Header("Default-Video"), Space(2f)]
        [SerializeField] private bool defaultVsync = false;
        [SerializeField] private bool defaultFullScreen = true;
        [SerializeField] private FullScreenMode defaultFullScreenMode = FullScreenMode.FullScreenWindow;
        [SerializeField] private int defaultResolutionWidth = 1920;
        [SerializeField] private int defaultResolutionHeight = 1080;
        [SerializeField] private bool defaultFramerateLimitEnabled = true;
        [SerializeField] private int defaultFramerateLimit = 90;

        [Header("Default-Graphic"), Space(2f)]
        [SerializeField] private MsaaQuality defaultMsaaQuality = MsaaQuality.Disabled;
        [SerializeField] private AntialiasingMode defaultAntialiasingMode = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        [SerializeField] private AntialiasingQuality defaultAntialiasingQuality = AntialiasingQuality.High;
        [SerializeField] private AnisotropicFiltering defaultAnisotropicFiltering = AnisotropicFiltering.Enable;

        //[Header("Input"), Space(2f)]

        [Header("Default-Lang"), Space(2f)]
        [SerializeField] private GameLanguage defaultLang = GameLanguage.Ger;

        [Header("Network"), Space(2f)]
        [SerializeField] private string defaultNetIP = "127.0.0.1";
        [SerializeField] private ushort defaultNetPort = 7777;

        #endregion

        #region PrivateFields

        private bool useVR;
        private bool showFps;
        private bool showLog;
        private float masterVolume = 1.0f;       
        private bool vSync;
        private bool fullScreen;
        private FullScreenMode fullScreenMode;
        private int resolutionHeight;
        private int resolutionWidth;
        private bool framerateLimitActive;
        private int framerateLimit;
        private MsaaQuality msaaQuality;
        private AntialiasingMode antialiasingMode;
        private AntialiasingQuality antialiasingQuality;
        private AnisotropicFiltering anisotropicFiltering;

        private GameLanguage language = GameLanguage.Ger;
        private string serverIP;
        private ushort serverPort;

        #endregion

        #region PublicFields

        public bool DoAudioSetup => doAudioSetup;
        public bool DoVideoSetup => doVideoSetup;
        public bool DoGraphicSetup => doGraphicSetup;
        public bool UseVR => useVR;
        public bool ShowFps => showFps;
        public bool ShowLog => showLog;
        public float MasterVolume => masterVolume;  
        public bool VSync => vSync;
        public bool FullScreen => fullScreen;
        public FullScreenMode FullScreenMode => fullScreenMode;
        public int ResolutionHeight => resolutionHeight;
        public int ResolutionWidth => resolutionWidth;
        public bool FramerateLimitActive => framerateLimitActive;
        public int FramerateLimit => framerateLimit;
        public MsaaQuality MsaaQuality => msaaQuality;
        public AntialiasingMode AntialiasingMode => antialiasingMode;
        public AntialiasingQuality AntialiasingQuality => antialiasingQuality;
        public AnisotropicFiltering AnisotropicFiltering => anisotropicFiltering;
        public GameLanguage CurrentLanguage => language;
        public string NetIP => serverIP;
        public ushort NetPort => serverPort;

        [Space(50f)]
        public bool loadDone = false;

        #endregion

        #region SetValues

        public void SetMasterVolume(float value)
        {
            masterVolume = value;
        }

        public void SetLang(GameLanguage lng)
        {
            language = lng;
        }

        public void SetVsync(bool state)
        {
            vSync = state;
        }

        public void SetFullScreen(bool state)
        {
            fullScreen = state;
        }

        public void SetResolution(int height, int width)
        {
            resolutionHeight = height;
            resolutionWidth = width;
        }

        public void SetMsaa(MsaaQuality msaa)
        {
            msaaQuality = msaa;
        }

        public void SetAntiAntialiasingMode(AntialiasingMode mode)
        {
            antialiasingMode = mode;
        }

        public void SetAntialiasingQuality(AntialiasingQuality quality)
        {
            antialiasingQuality = quality;
        }

        public void SetIP(string ip)
        {
            serverIP = ip;
        }

        public void SetPort(string port)
        {
            serverPort = ushort.Parse(port);
        }

        public void SetFramrateLimit(bool enabled, int value)
        {
            framerateLimitActive = enabled;
            framerateLimit = value;
        }

        #endregion

        #region LoadAndSave

        public bool Start()
        {
            if (loadDone)
            {
                Debug.Log("<color=#7D4EFF>Config Values Already loaded - Skip...</color>");
                return loadDone;
            }

            if (dontCreateConfigFile)
            {
                Debug.Log("<color=#7D4EFF>Skip Loading Config Values - Use Default Values</color>");
                SetDefaults();
                loadDone = true;
                return loadDone;
            }

            if (Serialization.FileExists(configname))
            {
                Debug.Log("<color=#7D4EFF>Loading Config Values from File ...</color>");
                LoadConfigValues();
                loadDone = true;
                return loadDone;
            }

            Debug.Log("<color=#7D4EFF>ConfigFile Does not exist -> creating a new file</color>");
            SetDefaults();
            SaveConfigValues();
            loadDone = true;
            return loadDone;
        }

        public void LoadConfigValues()
        {
            string file = configname;
            if (usePersistentDataPath)
            {
                file = Application.persistentDataPath + "\\" + configname;
            }

            List<string> fileContent = Serialization.LoadTextByLine(file);
            Dictionary<string, string> configValues = new Dictionary<string, string>();

            foreach (var item in fileContent)
            {
                if(string.IsNullOrEmpty(item)) continue;
                if(item.Contains("//") || item.Contains("---")) continue;
                string[] temp = item.Split('='); 
                configValues.Add(temp[0], temp[1]);
            }

            useVR = byte.TryParse(configValues[nameof(useVR)], out byte result) ? Convert.ToBoolean(result) : defaultUseVR;
            showFps = byte.TryParse(configValues[nameof(showFps)], out result) ? Convert.ToBoolean(result) : defaultShowFPS;
            showLog = byte.TryParse(configValues[nameof(showLog)], out result) ? Convert.ToBoolean(result) : defaultShowLog;

            masterVolume = float.TryParse(configValues[nameof(masterVolume)], out float resultFloat) ? resultFloat : defaultMasterVolume;

            vSync = byte.TryParse(configValues[nameof(vSync)], out result) ? Convert.ToBoolean(result) : defaultVsync;
            fullScreen = byte.TryParse(configValues[nameof(fullScreen)], out result) ? Convert.ToBoolean(result) : defaultFullScreen;
            resolutionHeight = int.TryParse(configValues[nameof(resolutionHeight)], out int resultInt) ? resultInt : defaultResolutionHeight;
            resolutionWidth = int.TryParse(configValues[nameof(resolutionWidth)], out resultInt) ? resultInt : defaultResolutionWidth;
            framerateLimitActive = byte.TryParse(configValues[nameof(framerateLimitActive)], out result) ? Convert.ToBoolean(result) : defaultFramerateLimitEnabled;
            framerateLimit = int.TryParse(configValues[nameof(framerateLimit)], out resultInt) ? resultInt : defaultFramerateLimit;

            msaaQuality = byte.TryParse(configValues[nameof(msaaQuality)], out result) ? (MsaaQuality)result : defaultMsaaQuality;
            antialiasingMode = byte.TryParse(configValues[nameof(resolutionWidth)], out result) ? (AntialiasingMode)result : defaultAntialiasingMode;
            antialiasingQuality = byte.TryParse(configValues[nameof(resolutionWidth)], out result) ? (AntialiasingQuality)result : defaultAntialiasingQuality;

            if (configValues.ContainsKey(nameof(serverIP)))
            {
                serverIP = configValues[nameof(serverIP)];
            }
            else
            {
                serverIP = defaultNetIP;
            }
            serverPort = ushort.TryParse(configValues[nameof(serverPort)], out ushort resultUshort) ? resultUshort : defaultNetPort;
        }

        public void SaveConfigValues()
        {
            string content = string.Empty;

            content += nameof(useVR) + "=" + Convert.ToByte(useVR) + "\n";
            content += nameof(showFps) + "=" + Convert.ToByte(showFps) + "\n";
            content += nameof(showLog) + "=" + Convert.ToByte(showLog) + "\n";
            content += nameof(masterVolume) + "=" + masterVolume.ToString() + "\n";
            content += nameof(vSync) + "=" + Convert.ToByte(vSync) + "\n";
            content += nameof(fullScreen) + "=" + Convert.ToByte(fullScreen) + "\n";
            content += nameof(resolutionHeight) + "=" + resolutionHeight.ToString() + "\n";
            content += nameof(resolutionWidth) + "=" + resolutionWidth.ToString() + "\n";
            content += nameof(framerateLimitActive) + "=" + Convert.ToByte(framerateLimitActive) + "\n";
            content += nameof(framerateLimit) + "=" + framerateLimit.ToString() + "\n";
            content += nameof(msaaQuality) + "=" + Convert.ToByte((int)msaaQuality) + "\n";
            content += nameof(antialiasingMode) + "=" + Convert.ToByte((int)antialiasingMode) + "\n";
            content += nameof(antialiasingQuality) + "=" + Convert.ToByte((int)antialiasingQuality) + "\n";
            content += nameof(serverIP) + "=" + serverIP + "\n";
            content += nameof(serverPort) + "=" + serverPort.ToString() + "\n";

            string file = configname;
            if( usePersistentDataPath )
            {
                file = Application.persistentDataPath + "\\" + configname;
            }
            Serialization.SaveText(content, file);
        }

        public void SetDefaults()
        {
            Debug.Log("Set default values");

            useVR = defaultUseVR;
            showFps = defaultShowFPS;
            showLog = defaultShowLog;

            masterVolume = defaultMasterVolume;

            vSync = defaultVsync;
            fullScreen = defaultFullScreen;
            fullScreenMode = defaultFullScreenMode;
            resolutionHeight = defaultResolutionHeight;
            resolutionWidth = defaultResolutionWidth;
            framerateLimit = defaultFramerateLimit;
            framerateLimitActive = defaultFramerateLimitEnabled;

            antialiasingMode = defaultAntialiasingMode;
            antialiasingQuality = defaultAntialiasingQuality;
            anisotropicFiltering = defaultAnisotropicFiltering;
            language = defaultLang;

            serverIP = defaultNetIP;
            serverPort = defaultNetPort;
        }

        #endregion
    }
}

