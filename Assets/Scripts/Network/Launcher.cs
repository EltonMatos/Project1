using System;
using Menu;
using Menu.Screens;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace Network
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public static Launcher Instance;

        public ErrorInfo PhotonErrorInfo;

        public Action<Player> OnPhotonPlayerJoinedRoom;

        private void Awake()
        {
            Instance = this;
        }

        /* Functional methods */
        public void ConnectToLobby(TMP_InputField nicknameNameInput)
        {
            if (string.IsNullOrEmpty(nicknameNameInput.text)) return;
            
            PhotonNetwork.LocalPlayer.NickName = nicknameNameInput.text;
            PhotonNetwork.ConnectUsingSettings();
            MenuManager.Instance.OpenMenu("Loading");
        }
        
        public void CreateOrJoinRoom(TMP_InputField roomNameInput)
        {
            if (string.IsNullOrEmpty(roomNameInput.text)) return;

            PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
            MenuManager.Instance.OpenMenu("Loading");
        }

        /* Callback methods */
        public override void OnConnectedToMaster()
        {
            print("Connected to master");
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            print("Connected to lobby");
            MenuManager.Instance.OpenMenu("Lobby");
        }
        
        public override void OnJoinedRoom()
        {
            print("You joined " + PhotonNetwork.CurrentRoom.Name + " room with the username: " + PhotonNetwork.LocalPlayer.NickName);
            MenuManager.Instance.OpenMenu("Room");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            OnPhotonPlayerJoinedRoom?.Invoke(newPlayer);
        }

        public override void OnLeftRoom()
        {
            MenuManager.Instance.OpenMenu("Lobby");
        }

        public override void OnErrorInfo(ErrorInfo errorInfo)
        {
            PhotonErrorInfo = errorInfo;
            MenuManager.Instance.OpenMenu("Error");
        }
    }
}
