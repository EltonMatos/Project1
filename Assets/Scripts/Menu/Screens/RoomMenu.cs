using System;
using Network;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Menu.Screens
{
    public class RoomMenu : Menu
    {
        public TMP_Text roomNameText;
        public GameObject startGameButton;
        public Transform playerListContent;
        public GameObject playerListItemPrefab;

        private void OnEnable()
        {
            GameConnection.Instance.OnPhotonPlayerJoinedRoom += OnPhotonPlayerJoinedRoom;
            GameConnection.Instance.OnPhotonMasterClientSwitched += OnPhotonMasterClientSwitched;
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
            
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                OnPhotonPlayerJoinedRoom(player);
            }
            
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        private void OnDisable()
        {
            GameConnection.Instance.OnPhotonPlayerJoinedRoom -= OnPhotonPlayerJoinedRoom;
            GameConnection.Instance.OnPhotonMasterClientSwitched -= OnPhotonMasterClientSwitched;
        }

        private void OnPhotonMasterClientSwitched()
        {
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        
        public void StartGame()
        {
            //TODO pick track
            PhotonNetwork.LoadLevel("Track 2");
        }

        private void OnPhotonPlayerJoinedRoom(Player newPlayer)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
        }
    }
}