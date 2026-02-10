/// <author>Thomas Krahl</author>

using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using eecon_lab.Main;
using Unity.Netcode.Transports.UTP;
using eecon_lab.UI.Network;
using UnityEngine.UI;
using eecon_lab.vipr.video;
using eccon_lab.vipr.experiment;

namespace eecon_lab.Network
{
    public class NetworkManagement : MonoBehaviour
    {
        public NetworkManager networkManager;
        public UnityTransport unityTransport;
        public NetworkConnector networkConnector;
        public ClientData myClientData;

        [Header("Host")]
        public GameObject clientUiEntryPrefab;
        public Transform clientListUiRoot;
        public TextMeshProUGUI textFieldClientsTotal;
        public InfoMessageHandler messageHandler;
        public TextMeshProUGUI textFieldIp;
        public Button playButton;
        public TextMeshProUGUI textFieldButtonPlay;
        public List<ClientData> clients = new List<ClientData>();

        private CustomVideoPlayer.VideoPlayerState currentState;

        void Start()
        {
            myClientData = new ClientData();
            myClientData.clientName = System.Environment.MachineName;
            myClientData.clientIP = GetLocalIp();

            if(NetworkManager.Singleton == null) return;
            unityTransport = NetworkManager.Singleton.GetComponentInChildren<UnityTransport>();
            networkManager = NetworkManager.Singleton;
        }

        private void OnDestroy()
        {
        }

        public void Initialize()
        {
            Debug.Log("initialize Network Setup");
            Game.GameMode currentMode = Game.Instance.ActiveGameMode;

            switch (currentMode)
            {
                case Game.GameMode.unset:
                case Game.GameMode.normal:
                default:
                    return;

                case Game.GameMode.client:
                    SetClient();
                    break;
                case Game.GameMode.host:
                    SetHost();
                    break;
            }
        }

        private void SetClient()
        {
            bool success = NetworkManager.Singleton.StartClient();
            Debug.Log("Network Client Start success = " + success);
            if (!success)
            {
                Level.Instance.SceneLoader.PlayDirectorLoadSpecificScene(0);
                return;
            }
            Level.Instance.SetNetworkMode(true);
            StartCoroutine(Test());
        }

        public void DiconnectClient()
        {
            if(networkConnector == null) return;

            if (networkManager.IsClient)
            {
                networkConnector.DisconnectClientRpc();
            }

            if (networkManager.IsHost)
            {
                Debug.Log("HostShutDown");
                networkManager.Shutdown();
            } 
        }

        private IEnumerator Test()
        {
            yield return new WaitForSeconds(1f);
            networkConnector = GameObject.FindWithTag("NetworkSpecial").GetComponent<NetworkConnector>();
            if (networkConnector != null)
            {
                networkConnector.SentClientData(myClientData.clientName, myClientData.clientIP);
            } 
        }

        private void SetHost()
        {
            Debug.Log("H1");
            bool success = NetworkManager.Singleton.StartHost();
            Debug.Log("Network Host Start success = " + success);

            if (!success)
            {

                Level.Instance.SceneLoader.PlayDirectorLoadSpecificScene(0);
                return;
            }
            NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
            UpdateClientUI();
            if (textFieldIp != null) textFieldIp.text = "IP=" + GetLocalIp();
            currentState = CustomVideoPlayer.VideoPlayerState.stopped;
            Debug.Log("H2");
            Level.Instance.SetNetworkMode(true);
        }

        public void DisconnectClient(ulong clientId)
        {
            networkManager.DisconnectClient(clientId);
            RemoveClientData(clientId);
        }

        public void SetClientData(string name, string ip, ulong id)
        {
            var clientData = new ClientData();
            var ui = Instantiate(clientUiEntryPrefab, clientListUiRoot);

            clientData.clientID = id;
            clientData.clientName = name;
            clientData.clientIP = ip;
            clientData.clientUIentry = ui.GetComponent<ClientUiEntry>();
            clientData.playerState = CustomVideoPlayer.VideoPlayerState.none;

            clients.Add(clientData);
            ShowInfoText($"Client {name} Connected ({ip})");
            UpdateClientUI();
        }

        private void RemoveClientData(ulong id)
        {
            foreach (var clientData in clients)
            {
                if (clientData.clientID == id)
                {
                    var ui = clientData.clientUIentry;
                    Destroy(ui.gameObject);
                    ShowInfoText($"Client {clientData.clientName} Disconnected");
                    clients.Remove(clientData);
                    return;
                }
            }

        }

        private void ShowInfoText(string text)
        {
            messageHandler.AddMessage(text);
            //textFieldInfo.text = text;
            //textFieldInfo.gameObject.SetActive(true);
        }

        private void OnConnectionEvent(NetworkManager arg1, ConnectionEventData arg2)
        {
            Debug.Log("Connection event - Type=" + arg2.EventType + "/ ClientID=" + arg2.ClientId);


            switch (arg2.EventType)
            {
                case ConnectionEvent.ClientConnected:
                    break;

                case ConnectionEvent.PeerConnected:
                    break;

                case ConnectionEvent.ClientDisconnected:
                    RemoveClientData(arg2.ClientId);
                    break;

                case ConnectionEvent.PeerDisconnected:
                    break;
                default:
                    break;
            }
            UpdateClientUI();

        }

        public void UpdateClientUI()
        {
            foreach (var client in clients)
            {
                client.UpdateUI();    
            }
            textFieldClientsTotal.text = "Connected Clients = " + clients.Count.ToString();
        }

        public string GetLocalIp()
        {
            string ipAddress = string.Empty;

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddress = ip.ToString();
                    return ipAddress;
                }
            }
            throw new System.Exception("No Network Adapter with IPv4 address found");
        }

        public void SetHostConnectionData()
        {
            unityTransport.SetConnectionData(myClientData.clientIP, 7777);
        }

        public void SetClientConnectionData(string ip, string port)
        {
            if (ip == string.Empty)
            {
                Game.Instance.GameOptions.GetConfig().SetIP("127.0.0.1");
            }
            else
            {
                Game.Instance.GameOptions.GetConfig().SetIP(ip);
            }

            if (port == string.Empty)
            {
                Game.Instance.GameOptions.GetConfig().SetPort("7777");
            }
            else
            {
                Game.Instance.GameOptions.GetConfig().SetPort(port);
            }
            SetConnectionData();
            Game.Instance.GameOptions.GetConfig().SaveConfigValues();
        }

        public void SetConnectionData()
        {
            Debug.Log("Set Connection Data");
            unityTransport.SetConnectionData(Game.Instance.GameOptions.GetConfig().NetIP, Game.Instance.GameOptions.GetConfig().NetPort);
        }

        public void UpdateVideoPlayerStateClient(CustomVideoPlayer.VideoPlayerState state)
        {
            if(networkConnector == null) return;
            networkConnector.UpdateVideoPlayerStatusRpc(state);
        }

        public void UpdateVideoPlayerState(CustomVideoPlayer.VideoPlayerState state, ulong clientID)
        {
            Debug.Log("Update Video Player State - Client ID=" + clientID + " / state=" + state);
            ChangeCurrentState(state);

            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].clientID == clientID)
                {
                    clients[i].playerState = state;
                }
            }
            UpdateClientUI();
        }

        public void ChangeCurrentState(CustomVideoPlayer.VideoPlayerState state)
        {
            currentState = state;

            if (state == CustomVideoPlayer.VideoPlayerState.playing)
            {
                textFieldButtonPlay.text = "Stop";
            }
            else
            {
                textFieldButtonPlay.text = "Play";
            } 
        }



        public void StartExperiment()
        {
            
            string data = Level.Instance.ExperimentPlayer.GetSaveDataFromFileByDropDownValue();
            networkConnector.StartExperimentRpc(data);
            Debug.Log("StartExperiment");
        }


        public void TogglePlayerState()
        {
            if (currentState == CustomVideoPlayer.VideoPlayerState.playing)
            {
                currentState = CustomVideoPlayer.VideoPlayerState.stopped;
                textFieldButtonPlay.text = "Play";
                networkConnector.StopVideo();
            }
            else
            {

                currentState = CustomVideoPlayer.VideoPlayerState.playing;
                textFieldButtonPlay.text = "Stop";
                networkConnector.StartVideo();
            }
        }

        public CustomVideoPlayer.VideoPlayerState GetVideoPlayerState()
        {
            return currentState;
        }

        public void Shutdown()
        {
            networkManager.Shutdown();
        }

        public void ToggleClientXr(bool enabled)
        {
            networkConnector.ToggleXrRPc(enabled);
        }
    }
}


