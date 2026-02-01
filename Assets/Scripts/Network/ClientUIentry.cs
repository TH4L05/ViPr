/// <author>Thomas Krahl</author>

using UnityEngine;
using TMPro;

namespace eecon_lab.UI.Network
{
    public class ClientUIentry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textFieldClientID;
        [SerializeField] private TextMeshProUGUI textFieldClientName;
        [SerializeField] private TextMeshProUGUI textFieldClientIP;
        [SerializeField] private TextMeshProUGUI textFieldClientPlayerState;

        public void UpdateText(string id, string name, string ip, string state)
        {
            textFieldClientID.text = id;
            textFieldClientName.text = name;
            textFieldClientIP.text = ip;
            textFieldClientPlayerState.text = state;
        }

        public void UpdateID(string id)
        {
            textFieldClientID.text = id;
        }

        public void UpdateName(string name)
        {
            textFieldClientName.text = name;
        }

        public void UpdateIP(string ip)
        {
            textFieldClientIP.text = ip;
        }

        public void UpdateState(string state)
        {
            textFieldClientIP.text = state;
        }

    }
}

