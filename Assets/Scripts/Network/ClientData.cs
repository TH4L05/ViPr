/// <author>Thomas Krahl</author>

using eecon_lab.UI.Network;
using eecon_lab.vipr.video;
using UnityEditor;
using eccon_lab.vipr.experiment;

namespace eecon_lab.Network
{
    [System.Serializable]
    public class ClientData
    {
        public ulong clientID;
        public string clientName;
        public string clientIP;
        public ExperimentState experimentState;
        public ClientUiEntry clientUIentry;

        public void SetPlayerState(ExperimentState state = ExperimentState.Invalid)
        {
            experimentState = state;
            clientUIentry.UpdateState(experimentState.ToString());
        }

        public void SetXrState(string state)
        {
            clientUIentry.UpdateXrState(state);
        }

        public void UpdateUI()
        {
            clientUIentry.UpdateText(clientID.ToString(), clientName, clientIP, experimentState.ToString());
        }
    }
}

