/// <author>Thomas Krahl</author>

using UnityEngine;
using TMPro;
using eecon_lab.Main;
using eecon_lab.Network;
using Unity.Netcode;

namespace eecon_lab
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputFieldIP;
        [SerializeField] private TMP_InputField inputFieldPort;
        [SerializeField] private TextMeshProUGUI textFieldIpSelf;
        [SerializeField] private NetworkManagement networkManagement;

        private void Awake()
        {
            
        }

        private void Start()
        {
            textFieldIpSelf.text = "Your IP address: " + networkManagement.GetLocalIp();
            Game.Instance.SetGameMode(Game.GameMode.unset);
        }

        public void SetFromConnectionFromInputFields()
        {
            string ip = inputFieldIP.text;
            string port = inputFieldPort.text;
            networkManagement.SetClientConnectionData(ip, port);
            
        }

        public void GetNetDataFromConfig()
        {
            string ip = Game.Instance.GameOptions.GetConfig().NetIP;
            ushort port = Game.Instance.GameOptions.GetConfig().NetPort;

            inputFieldIP.text = ip;
            inputFieldPort.text = port.ToString();
        }
    }
}

