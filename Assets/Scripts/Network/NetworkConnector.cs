/// <author>Thomas Krahl</author>

using UnityEngine;
using Unity.Netcode;
using eecon_lab.Main;
using eecon_lab.vipr.video;
using eccon_lab.vipr.experiment;

namespace eecon_lab.Network
{
    public class NetworkConnector : NetworkBehaviour
    {
        private int videoIndex = 0;

        [Rpc(SendTo.NotServer)]
        public void TestRpc(string command)
        {
            Debug.Log("ServerMessage =" + command);
        }

        [Rpc(SendTo.Server)]
        public void DisconnectClientRpc(RpcParams prams = default)
        {
            Level.Instance.NetworkManagement.DisconnectClient(prams.Receive.SenderClientId);
        }

        [Rpc(SendTo.Server)]
        public void SetClientDataRpc(string name, string ip, RpcParams prams = default)
        {
            Debug.Log("SetClient");
            Level.Instance.NetworkManagement.SetClientData(name, ip, prams.Receive.SenderClientId);
        }

        [Rpc(SendTo.NotServer)]
        public void PlayVideoRpc(int index)
        {
            Debug.Log("Play Video");
            Level.Instance.CustomVideoPlayer.PrepareVideo(index);
        }

        [Rpc(SendTo.NotServer)]
        public void StopVideoRpc()
        {
            Debug.Log("Play Video");
            Level.Instance.CustomVideoPlayer.StopVideo();
        }

        [Rpc(SendTo.Server)]
        public void UpdateVideoPlayerStatusRpc(CustomVideoPlayer.VideoPlayerState state, RpcParams prams = default)
        {
            Level.Instance.NetworkManagement.UpdateVideoPlayerState(state, prams.Receive.SenderClientId);
        }

        [Rpc(SendTo.NotServer)]
        public void ToggleXrRPc(bool enabled)
        {
            Level.Instance.ToggleXr(enabled);
        }

        [Rpc(SendTo.NotServer)]
        public void StartExperimentRpc(string jsonString) 
        {
            Debug.Log("StartExperimentRPC");
            Level.Instance.ExperimentPlayer.CreateExperimentJson(jsonString);

        }



        public void UpdateVideoPlayerState(CustomVideoPlayer.VideoPlayerState state)
        {
            UpdateVideoPlayerStatusRpc(state);
        }


        public void SentClientData(string name, string ip)
        {
            SetClientDataRpc(name, ip);
        }

        public void SetVideoIndex(int index)
        {
            videoIndex = index;
        }

        public void StartVideo()
        {
            PlayVideoRpc(videoIndex);
        }

        public void StopVideo()
        {
            StopVideoRpc();
        }

    }
}

