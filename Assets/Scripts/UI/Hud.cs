/// <author>Thomas Krahl</author>

using UnityEngine;
using TMPro;
using eecon_lab.Main;
using eecon_lab.XR;
using eecon_lab.Main.Configuration;
using TK.Util;

namespace eecon_lab.UI
{
    public class Hud : MonoBehaviour
    {
        #region SerializedFields

        [Header("FPS"), Space(2.0f)]
        [SerializeField] private bool dislpayFPS = false;
        [SerializeField] private TextMeshProUGUI fpsTextField;

        #endregion

        #region PrivateFields

        private FPS fps;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            SetupUnityXR.OnXrStateChanged += Setup;
        }

        private void LateUpdate()
        {
            if (dislpayFPS) UpdateFPS();
        }

        private void OnDestroy()
        {
            SetupUnityXR.OnXrStateChanged -= Setup;
        }

        #endregion

        #region Setup

        private void Setup(bool useVR)
        {
            Debug.Log("<color=#AF870C>Setup UI</color>");
            fps = new FPS();
            dislpayFPS = Game.Instance.GameOptions.GetConfig().ShowFps;
            ShowFPS(dislpayFPS);

        }
        
        #endregion

        #region FPS

        public void ToggleFPS()
        {
            dislpayFPS = !dislpayFPS;
            ShowFPS(dislpayFPS);
        }

        public void ShowFPS(bool show)
        {
            dislpayFPS = show;
            if (fpsTextField != null) fpsTextField.gameObject.SetActive(dislpayFPS);
        }

        private void UpdateFPS()
        {
            if (!dislpayFPS) return;
            float frames = Mathf.Ceil(fps.GetFps(Time.deltaTime));
            if (fpsTextField != null) fpsTextField.text = "FPS: " + frames.ToString();
        }

        #endregion
    }
}

