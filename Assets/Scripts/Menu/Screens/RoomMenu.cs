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
        public Transform playerListContent;
        public GameObject playerListItemPrefab;

        private void OnEnable()
        {
            Launcher.Instance.OnPhotonPlayerJoinedRoom += OnPhotonPlayerJoinedRoom;
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
            
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                OnPhotonPlayerJoinedRoom(player);
            }
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void OnPhotonPlayerJoinedRoom(Player newPlayer)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
        }
    }
}