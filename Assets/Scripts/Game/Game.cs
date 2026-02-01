/// <author>Thomas Krahl</author>

using System;
using UnityEngine;
using UnityEngine.Audio;
using eecon_lab.Main.Configuration;
using eecon_lab.Utilities;

namespace eecon_lab.Main
{
    public class Game : MonoBehaviour
    {
        public enum GameMode
        {
            unset = -1,
            normal,
            client,
            host,
            editor,
        }

        #region SerializedFields

        [Header("References")]
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private Level level;
        [SerializeField] private AudioMixer audioMixer;
        

        [Header("Options")]
        [SerializeField] private bool dontDestroyOnLoad = true;
        [SerializeField, Range(0.0f, 5.0f)] private float timeScale = 1f;
        [SerializeField] GameMode gameMode = GameMode.unset;

        #endregion

        #region PublicFields

        public static Game Instance;
        public Level Level => level;
        public GameOptions GameOptions => gameOptions;
        public bool VRactive { get; set; }
        public GameMode ActiveGameMode => gameMode;

        #endregion

        #region PrivateFields

        private GameOptions gameOptions;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            Initialize();
        }

        private void OnApplicationQuit()
        {
            gameConfig.loadDone = false;
        }

        #endregion

        #region Setup

        private void Initialize()
        {
            gameOptions = new GameOptions();
            gameOptions.Initialize(gameConfig, audioMixer);

            SetTimeScale(timeScale);
        }

        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
        }

        public void SetLevel(Level l)
        {
            level = l;
        }

        #endregion

        

        public void SetGameMode(int mode)
        {
            gameMode = (GameMode)mode;
        }

        public void SetGameMode(GameMode mode)
        {
            gameMode = mode;
        }
    }
}
