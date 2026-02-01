/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace eecon_lab.Utilities
{
    public class IngameLog : MonoBehaviour
    {
        [System.Serializable]
        public struct LogMessage
        {
            public string content;
            public string stagTrace;
            public LogType type;
        }

        #region SerializedFields

        [SerializeField] private bool isVisibleOnStart = true;
        [SerializeField] private bool clearMessagesOnSceneStart = true;
        [SerializeField, Range(1, 50)] private int maxMessages = 10;
        [SerializeField] private LogType[] activeLogTypes = { LogType.Log, LogType.Warning, LogType.Error };
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField] private List<LogMessage> messages = new List<LogMessage>();

        #endregion

        #region PrivateFields

        private bool active;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            if (textField != null) textField.text = "";
            Application.logMessageReceived += LogMessageReceived;
        }

        private void Start()
        {
            active = isVisibleOnStart;
            if (clearMessagesOnSceneStart) messages.Clear();
            textField.transform.parent.gameObject.SetActive(active);
        }

        private void OnDestroy()
        {
            Application.logMessageReceived += LogMessageReceived;
        }

        #endregion

        #region MessageHandle

        private void LogMessageReceived(string content, string stackTrace, LogType type)
        {
            bool activeLogType = false;
            foreach (var logType in activeLogTypes)
            {
                if (logType == type)
                {
                    activeLogType = true;
                    break;
                }
            }

            if (!activeLogType) return;
            AddMessage(content, stackTrace, type);
        }

        public void AddMessage(string message, string stacktrace, LogType type)
        {
            string content = message;

            switch (type)
            {
                case LogType.Error:
                    content = "<color=#FF0000>" + message + "</color>";
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    content = "<color=#FFD800>" + message + "</color>";
                    break;
                case LogType.Log:
                    break;
                case LogType.Exception:
                    content = "<color=#FF0000>" + message + "</color>";
                    break;
                default:
                    break;
            }

            LogMessage newMessage = new LogMessage()
            {
                content = content,
                stagTrace = stacktrace,
                type = type,
            };

            if (messages.Count >= maxMessages)
            {
                DeleteOldestMessage();
            }
            messages.Add(newMessage);
            UpdateMessageView();
        }

        public void ClearMessages()
        {
            messages.Clear();
        }

        private void DeleteOldestMessage()
        {
            if (messages.Count < 1) return;
            messages.RemoveAt(0);
        }

        #endregion

        #region View

        public void UpdateMessageView()
        {
            if (textField == null) return;

            string content = "";
            foreach (LogMessage message in messages)
            {
                content += message.content;
                content += "\n";
            }
            textField.text = content;
        }

        public void ChangeVisbilityState()
        {
            active = !active;
            if (textField != null) textField.transform.parent.gameObject.SetActive(active);
        }

        public void ShowLog(bool show)
        {
            active = show;
            if (textField != null) textField.transform.parent.gameObject.SetActive(active);
        }

        #endregion
    }
}




