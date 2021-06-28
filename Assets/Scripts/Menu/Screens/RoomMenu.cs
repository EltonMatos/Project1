using System;
using Network;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Screens
{
    public class RoomMenu : Menu
    {
        public TMP_Text roomNameText;
        public GameObject startGameButton;
        public GameObject trackCustomizationPanel;
        public Transform playerListContent;
        public GameObject playerListItemPrefab;

        public Sprite[] imagemSp;        

        public Image trackCurrent;

        private int _idTrack;        

        private void Start()
        {
            _idTrack = 0;
            trackCurrent.sprite = imagemSp[_idTrack];
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
            trackCustomizationPanel.SetActive(PhotonNetwork.IsMasterClient);
        }


        private void OnDisable()
        {
            GameConnection.Instance.OnPhotonPlayerJoinedRoom -= OnPhotonPlayerJoinedRoom;
            GameConnection.Instance.OnPhotonMasterClientSwitched -= OnPhotonMasterClientSwitched;
        }

        private void OnPhotonMasterClientSwitched()
        {
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
            trackCustomizationPanel.SetActive(PhotonNetwork.IsMasterClient);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        
        public void StartGame()
        {            
            PhotonNetwork.LoadLevel("Track " + _idTrack);
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

        public void NextTrack()
        {
            if (_idTrack < imagemSp.Length - 1)
            {
                _idTrack += 1;
                trackCurrent.sprite = imagemSp[_idTrack];                                
            }
        }

        public void PreviusTrack()
        {
            if (_idTrack > 0)
            {
                _idTrack -= 1;
                trackCurrent.sprite = imagemSp[_idTrack];                                
            }
        }

    }
}
