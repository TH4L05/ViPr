/// <author>Thomas Krahl</author>

using UnityEngine;
using TMPro;
using eecon_lab.Main;
using eecon_lab.XR;
using eecon_lab.Main.Configuration;

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

        private float dt;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            SetupUnityXR.OnInitFinished += Setup;
        }

        private void LateUpdate()
        {
            if (dislpayFPS) UpdateFPS();
        }

        private void OnDestroy()
        {
            SetupUnityXR.OnInitFinished -= Setup;
        }

        #endregion

        #region Setup

        private void Setup(bool useVR)
        {
            Debug.Log("<color=#AF870C>Setup UI</color>");
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
            float frames = Mathf.Ceil(CalculateFPS());
            if (fpsTextField != null) fpsTextField.text = "FPS: " + frames.ToString();
        }

        private float CalculateFPS()
        {
            dt += (Time.deltaTime - dt) * 0.1f;
            float frames = 1.0f / dt;
            frames = Mathf.Clamp(frames, 0.0f, 999f);
            return frames;
        }

        #endregion
    }
}

