using System;
using Network;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            // SceneManager.sceneLoaded += OnSceneLoaded;
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
            PhotonNetwork.LoadLevel(_raceScene);
        }

        private void OnPhotonPlayerJoinedRoom(Player newPlayer)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
        }
        
        // private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        // {
        //     if (scene.name == _raceScene)
        //     {
        //         var transform1 = transform;
        //         PhotonNetwork.Instantiate("PhotonNetworkPlayerPrefab", transform1.position, transform1.rotation);
        //     }
        // }
    }
}
