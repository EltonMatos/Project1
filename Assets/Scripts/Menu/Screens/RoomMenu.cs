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

        
        //TODO make this possible to change so user can pick track
        private readonly string _raceScene = "Track 1";

        private void OnEnable()
        {
            GameConnection.Instance.OnPhotonPlayerJoinedRoom += OnPhotonPlayerJoinedRoom;
            GameConnection.Instance.OnPhotonMasterClientSwitched += OnPhotonMasterClientSwitched;
            roomNameText.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
            
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
            PhotonNetwork.LoadLevel(_raceScene);
        }

        public void ChangeColor()
        {
            //get list of available colors
            //run RPC to update every player
        }

        private void OnPhotonPlayerJoinedRoom(Player newPlayer)
        {
            GameRoom.Instance.AddPlayer(newPlayer);
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
        }
        
    }
}
