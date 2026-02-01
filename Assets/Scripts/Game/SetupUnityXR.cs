/// <author>Thomas Krahl</author>

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

namespace eecon_lab.XR
{
    [CreateAssetMenu(fileName = "newXRsetup", menuName = "Data/xrSetup")]
    public class SetupUnityXR : ScriptableObject
    {
        [SerializeField] private bool initializeOnStart = true;
        public static Action<bool> OnInitFinished;
        private bool xrInitialized;

        public void OnLevelExit()
        {
            xrInitialized = false;
        }

        public void SkipInit()
        {
            Debug.Log("<color=#2AC93A>Skip XR Initialization</color>");
            xrInitialized = false;
            OnInitFinished?.Invoke(xrInitialized);
        }

        public void Initialize(MonoBehaviour caller)
        {
            if (!initializeOnStart)
            {
                xrInitialized = false;
                OnInitFinished?.Invoke(xrInitialized);
                return;
            }

            if (!xrInitialized)
            {
                bool activeLoaderIsPresent = GetXRmanagerLoaderState();
                if (activeLoaderIsPresent)
                {
                    Debug.Log("<color=#2AC93A>XR already Initialized!.</color>");
                    StopXR();
                }
                caller.StartCoroutine(InitializeUXR());
            }
            else
            {
                Debug.Log("<color=#2AC93A>XR already Initialized!.</color>");
                OnInitFinished?.Invoke(xrInitialized);  
            }                       
        }

        private bool GetXRmanagerLoaderState()
        {
            if (XRGeneralSettings.Instance.Manager.activeLoader == null) return false;
            return true;
        }

        private IEnumerator InitializeUXR()
        {
            Debug.Log("<color=#2AC93A>Start XR initialization ...</color>");
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

            xrInitialized = GetXRmanagerLoaderState();
            Debug.Log("LoaderState = " + xrInitialized);
            if (xrInitialized)
            {
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                Debug.Log("<color=#2AC93A>Initializing XR Success.</color>");
                Debug.Log("Loaded XR device = " + XRSettings.loadedDeviceName + "(" + XRSettings.eyeTextureWidth + "x" + XRSettings.eyeTextureWidth + ")");
            }
            else
            {
                Debug.Log("<color=#FF0000>Initializing XR Failed.</color>");
            }
            OnInitFinished?.Invoke(xrInitialized);
        }

        private void SetGameViewRenderMode(GameViewRenderMode mode)
        {
            XRSettings.gameViewRenderMode = mode;
            Debug.Log("Game view render mode = " + XRSettings.gameViewRenderMode);
        }

        public void StopXR()
        {
            Debug.Log("<color=#2AC93A>Stopping XR ...</color>");
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            xrInitialized =false;
            Debug.Log("<color=#2AC93A>XR stopped</color>");
        }
    }
}

