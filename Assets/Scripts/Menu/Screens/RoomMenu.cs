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

        private ListTrack track;

        //TODO make this possible to change so user can pick track
        private string _raceScene;

        private void Start()
        {
            track = GetComponentInChildren<ListTrack>();            
            _raceScene = "Track " + track.idTrack;
        }


        private void OnEnable()
        {
            GameConnection.Instance.OnPhotonPlayerJoinedRoom += OnPhotonPlayerJoinedRoom;
            GameConnection.Instance.OnPhotonMasterClientSwitched += OnPhotonMasterClientSwitched;
            roomNameText.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
            
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                print("Starting the menu player found " + player);
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
            GameRoom.Instance.ChangeCurrentPlayerColor();
        }

        private void OnPhotonPlayerJoinedRoom(Player newPlayer)
        {
            GameRoom.Instance.AddPlayer(newPlayer);
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
        }
        
    }
}
