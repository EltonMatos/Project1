using System;
using Network;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Results
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

        public void GotToRoom()
        {
            PhotonNetwork.LoadLevel("Menu");
        }

        private void GameFinished()
        {
            print("game finished, testing");
            resultsText.text = "";
            foreach (var gameResult in GameRoom.Instance.Results)
            {
                resultsText.text += $"#{gameResult.Position} {gameResult.Player} \n";
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