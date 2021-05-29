using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameConnection : MonoBehaviourPunCallbacks
{
    public Text chatLog;

    private void Awake()
    {
        chatLog.text += "\nConnecting to server...";
        //TODO get nickname from user
        PhotonNetwork.LocalPlayer.NickName = "Fernando_" + Random.Range(1, 100);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        chatLog.text += "\nConnected to server!";

        if (!PhotonNetwork.InLobby)
        {
            chatLog.text += "\nEntering lobby...";
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        chatLog.text += "\nConnected to lobby...";

        chatLog.text += "\nJoining test room...";
        //TODO get room name from screen
        PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        chatLog.text += "\nFailed to connect to lobby. Code: " + returnCode + ", message: " + message;
    }

    public override void OnJoinedRoom()
    {
        chatLog.text += "\nYou joined test room with the username: " + PhotonNetwork.LocalPlayer.NickName;
        //TODO instanciate player
    }
    
    public override void OnLeftRoom()
    {
        chatLog.text += "\nYou left the test room.";
        //TODO destroy player object
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        chatLog.text += "\nPlayer " + player.NickName + " joined the test room!";
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        chatLog.text += "\nPlayer " + player.NickName + " left the test room.";
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        chatLog.text += "\nSomething went wrong: " + errorInfo.Info;
    }
}
