/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using eecon_lab.Main.Configuration;
using eecon_lab.Character.Player;
using eecon_lab.Network;
using eecon_lab.Utilities;
using eecon_lab.vipr.video;
using eccon_lab.vipr.experiment;
using eecon_lab.XR;
using TK.SceneLoading;

namespace eecon_lab.Main
{
    public class Level : MonoBehaviour
    {
        public UnityEvent OnLevelStart;

        [Header("References")]
        [SerializeField] private SetupUnityXR xrSetup;
        [SerializeField] private SceneLoad sceneLoad;
        [SerializeField] private IngameLog ingameLog;
        [SerializeField] private Player player;
        [SerializeField] private List<ReflectionProbe> reflectionProbes = new List<ReflectionProbe>();
        [SerializeField] private NetworkManagement networkManagement;
        [SerializeField] private CustomVideoPlayer customVideoPlayer;
        [SerializeField] private ExperimentPlayer experimentPlayer;
        [SerializeField] private Camera[] cameras;

        [Header("Option")]
        [SerializeField] private bool skipXrSetup = false;
        [SerializeField] private bool skipGraphicSetup = false;

        [Header("Dev")]
        [SerializeField] private GameObject testCamera;
    
        #region PublicFields

        public static Level Instance;
        public SceneLoad SceneLoader => sceneLoad;
        public Player Player => player;
        public NetworkManagement NetworkManagement => networkManagement;
        public CustomVideoPlayer CustomVideoPlayer => customVideoPlayer;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            xrSetup.OnLevelExit();
            SetupUnityXR.OnInitFinished -= XRInitFinished;
        }

        #endregion

        #region Setup

        private void Initialize()
        {
            Game.Instance.SetLevel(this);
            if (testCamera != null) testCamera.SetActive(false);

            if (skipXrSetup || Game.Instance.ActiveGameMode == Game.GameMode.host || Game.Instance.ActiveGameMode == Game.GameMode.editor)
            {
                XRInitFinished(false);
                return;
            }
            SetupXR();
            if (ingameLog != null) ingameLog.ShowLog(Game.Instance.GameOptions.GetConfig().ShowLog);
        }

        public void SetupXR()
        {
            if (skipXrSetup)
            {
                XRInitFinished(false);
                return;
            }

            GameConfig config = Game.Instance.GameOptions.GetConfig();
            if (config != null && config.UseVR)
            {
                SetupUnityXR.OnInitFinished += XRInitFinished;

                if (xrSetup == null)
                {
                    Debug.LogError("<color=#FF0000>XR setup component is missing !!</color>");
                    XRInitFinished(false);
                    return;
                }
                
                xrSetup.Initialize(this);
                return;
            }
            XRInitFinished(false);
        }

        private void XRInitFinished(bool isInitialized)
        {
            Debug.Log($"<color=#AF870C>XR is initialized = {isInitialized}</color>");
            Game.Instance.VRactive = isInitialized;
            StartSetup();
            NetworkSetup();
            OnLevelStart?.Invoke();
        }

        private void StartSetup()
        {
            Debug.Log($"<color=#AF870C>Level Setup</color>");

            if (reflectionProbes.Count > 0)
            {
                foreach (var probe in reflectionProbes)
                {
                    probe.RenderProbe();
                }
            }
            Game.GameMode mode = Game.Instance.ActiveGameMode;
            Debug.Log("Gamemode = " + mode);

            /*switch (mode)
            {
                case Game.GameMode.unset:
                    break;
                case Game.GameMode.normal:
                case Game.GameMode.client:
                    Game.Instance.GameOptions.GetConfig().SetAntiAntialiasingMode(UnityEngine.Rendering.Universal.AntialiasingMode.TemporalAntiAliasing);
                    Game.Instance.GameOptions.GetConfig().SetMsaa(UnityEngine.Rendering.Universal.MsaaQuality.Disabled);
                    break;
                case Game.GameMode.host:
                    break;
                case Game.GameMode.editor:
                    Game.Instance.GameOptions.GetConfig().SetAntiAntialiasingMode(UnityEngine.Rendering.Universal.AntialiasingMode.None);
                    Game.Instance.GameOptions.GetConfig().SetMsaa(UnityEngine.Rendering.Universal.MsaaQuality._4x);
                    break;
                default:
                    break;
            }
            Game.Instance.GameOptions.SetSceneCameras(cameras);
            Game.Instance.GameOptions.GraphicSetup();*/
        }

        private void NetworkSetup()
        {
            if(networkManagement == null) return;
            if (Game.Instance.ActiveGameMode == Game.GameMode.normal || Game.Instance.ActiveGameMode == Game.GameMode.editor) return;
            networkManagement.Initialize();            
        }

        public void SetGameMode(int mode)
        {
            Game.Instance.SetGameMode(mode);
        }

        #endregion

        private void ToggleCursorState(CursorLockMode lockMode)
        {
            Cursor.lockState = lockMode;
        }

        public void ToggleLogVisibility()
        {
            ingameLog.ChangeVisbilityState();
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}
