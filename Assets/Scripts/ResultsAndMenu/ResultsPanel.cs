using System;
using Network;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace ResultsAndMenu
{
    public class ResultsPanel : MonoBehaviour
    {
        public TMP_Text resultsText;
        public GameObject goToRoomButton;

        private void OnEnable()
        {
            GameConnection.Instance.OnPhotonMasterClientSwitched += OnPhotonMasterClientSwitched;
            goToRoomButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        private void OnDisable()
        {
            GameConnection.Instance.OnPhotonMasterClientSwitched -= OnPhotonMasterClientSwitched;
        }

        private void OnPhotonMasterClientSwitched()
        {
            goToRoomButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public void GoToRoom()
        {
            PhotonNetwork.LoadLevel("Post Race Menu");
        }

        private void GameFinished()
        {
            print("game finished, testing");
            resultsText.text = "";
            foreach (var gameResult in GameRoom.Instance.Results)
            {
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (player.ActorNumber == gameResult.Player.ActorNumber)
                    {
                        resultsText.text += $"#{gameResult.Position} - {gameResult.Player.Color} - {player.NickName} \n\n";
                    }
                }
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            GameFinished();
        }
    }
}