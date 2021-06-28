using Network;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Menu.Screens
{
    public class LandingMenu : Menu
    {
        //TODO duplicate nickname should not be allowed
        public void ConnectToLobby(TMP_InputField nicknameInput)
        {
            GameConnection.Instance.ConnectToLobby(nicknameInput.text);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        
        private void OnEnable()
        {
            if (!GameConnection.Instance.disconnectWarningRead && GameConnection.Instance.disconnectionCause != null)
            {
                if (GameConnection.Instance.disconnectionCause == DisconnectCause.Exception)
                {
                    print("something went wrong with your connection");
                }

                if (GameConnection.Instance.disconnectionCause == DisconnectCause.DnsExceptionOnConnect)
                {
                    print("Could not connect to server");
                }
            }
        }
    }
}