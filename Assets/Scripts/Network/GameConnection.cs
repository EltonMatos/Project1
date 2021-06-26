﻿using System;
using Menu;
using Menu.Screens;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

namespace Network
{
    public class GameConnection : MonoBehaviourPunCallbacks
    {
        public static GameConnection Instance;

        public ErrorInfo PhotonErrorInfo;

        public Action<Player> OnPhotonPlayerJoinedRoom;
        public Action<Player> OnPhotonPlayerLeftRoom;
        public Action OnPhotonMasterClientSwitched;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /* Functional methods */
        public void ConnectToLobby(string nicknameName)
        {
            if (string.IsNullOrEmpty(nicknameName)) return;

            PhotonNetwork.LocalPlayer.NickName = nicknameName;
            PhotonNetwork.ConnectUsingSettings();
            MenuManager.Instance.OpenMenu("Loading");
        }

        public void CreateOrJoinRoom(string roomName)
        {
            if (string.IsNullOrEmpty(roomName)) return;

            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions {MaxPlayers = 4}, TypedLobby.Default);
            MenuManager.Instance.OpenMenu("Loading");
        }

        /* Callback methods */
        public override void OnConnectedToMaster()
        {
            print("Connected to master");
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.JoinLobby();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            OnPhotonMasterClientSwitched?.Invoke();
        }

        public override void OnJoinedLobby()
        {
            print("Connected to lobby");
            MenuManager.Instance.OpenMenu("Lobby");
        }

        public override void OnJoinedRoom()
        {
            print("You joined " + PhotonNetwork.CurrentRoom.Name + " room with the username: " +
                  PhotonNetwork.LocalPlayer.NickName);
            MenuManager.Instance.OpenMenu("Room");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            OnPhotonPlayerJoinedRoom?.Invoke(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            OnPhotonPlayerLeftRoom?.Invoke(otherPlayer);
        }

        public override void OnLeftRoom()
        {
            if (!SceneManager.GetActiveScene().name.Contains("Track"))
            {
                MenuManager.Instance.OpenMenu("Lobby");
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            print("Disconnected");
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                MenuManager.Instance.OpenMenu("Landing");
            }
            else
            {
                GameRoom.Instance.DestroySelf();
                SceneManager.LoadScene("Menu");
            }
        }

        public override void OnErrorInfo(ErrorInfo errorInfo)
        {
            PhotonErrorInfo = errorInfo;
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                MenuManager.Instance.OpenMenu("Error");
            }
            else
            {
                GameRoom.Instance.DestroySelf();
                SceneManager.LoadScene("Menu");
            }
        }
    }
}