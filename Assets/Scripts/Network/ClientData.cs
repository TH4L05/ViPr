/// <author>Thomas Krahl</author>

using eecon_lab.UI.Network;
using eecon_lab.vipr.video;

namespace eecon_lab.Network
{
    [System.Serializable]
    public class ClientData
    {
        public ulong clientID;
        public string clientName;
        public string clientIP;
        public CustomVideoPlayer.VideoPlayerState playerState;
        public ClientUIentry clientUIentry;

        public void SetPlayerState(CustomVideoPlayer.VideoPlayerState state = CustomVideoPlayer.VideoPlayerState.none)
        {
            playerState = state;
            UpdateUI();
        }

        public void UpdateUI()
        {
            clientUIentry.UpdateText(clientID.ToString(), clientName, clientIP, playerState.ToString());
        }
    }
}

