using System;
using Network;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Menu.Screens
{
    public class LandingMenu : Menu
    {
        public TMP_Text disconnectionText;
        public GameObject disconnectionInfoPanel;

        private void Start()
        {
            disconnectionInfoPanel.SetActive(false);
            UpdateDisconnectionInformation();
        }

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
          UpdateDisconnectionInformation();
        }

        private void UpdateDisconnectionInformation()
        {
            if (!GameConnection.Instance.disconnectWarningRead && GameConnection.Instance.disconnectionCause != null)
            {
                disconnectionInfoPanel.SetActive(true);
                if (GameConnection.Instance.disconnectionCause == DisconnectCause.Exception)
                {
                    disconnectionText.text = "Something went wrong, you got disconnected, please check your internet connection.";
                } 

                if (GameConnection.Instance.disconnectionCause == DisconnectCause.DnsExceptionOnConnect)
                {
                    disconnectionText.text = "Could not connect to server, please check your internet connection.";
                }
            }
        }

        public void CloseDisconnectionInfo()
        {
            disconnectionInfoPanel.SetActive(false);
        }
    }
}