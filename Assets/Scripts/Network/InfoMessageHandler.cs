/// <author>Thomas Krahl</author>


using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace eecon_lab.Network
{
    public class InfoMessageHandler : MonoBehaviour
    {
        [System.Serializable]
        public struct InfoMessage
        {
            public string text;

            public InfoMessage(string text)
            {
                this.text = text;
            }
        }

        [SerializeField] private List<InfoMessage> messages = new List<InfoMessage>();
        [SerializeField] private TextMeshProUGUI messageTextField;
        [SerializeField] private int maxMessages = 100;

        void Start()
        {
            messageTextField.text = "";
        }

        public void AddMessage(string text)
        {
            InfoMessage message = new InfoMessage(text);
            messages.Add(message);
            if (messages.Count > maxMessages)
            {
                DeleteFirstMessage();
                return;
            }
            UpdateTextField();
        }

        private void DeleteFirstMessage()
        {
            messages.RemoveAt(0);
            UpdateTextField();
        }

        private void UpdateTextField()
        {
            string text = "";
            foreach (var message in messages)
            {
                text += message.text;
                text += "\n";
            }
            messageTextField.text = text;
        }

        private void ClearMessages()
        {
            messages.Clear();
            messageTextField.text = "";
        }
    }
}

