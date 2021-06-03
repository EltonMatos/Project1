using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameConnection : MonoBehaviourPunCallbacks
{
    private int _totalPlayers = 0;
    
    private void Awake()
    {
        Debug.Log("Connecting to server...");
        //TODO get nickname from user
        PhotonNetwork.LocalPlayer.NickName = "Fernando_" + Random.Range(1, 100);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server!");

        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("Entering lobby...");
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to lobby...");

        Debug.Log("Joining test room...");
        //TODO get room name from screen
        PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to connect to lobby. Code: " + returnCode + ", message: " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("You joined test room with the username: " + PhotonNetwork.LocalPlayer.NickName);
        Transform positionCar1 = GameManager.Instance.carPositions[_totalPlayers];
        PhotonNetwork.Instantiate("Player", positionCar1.position, positionCar1.rotation);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("You left the test room.");
        //TODO destroy player object
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        Debug.Log("Player " + player.NickName + " joined the test room!");
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.Log("Player " + player.NickName + " left the test room.");
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        Debug.Log("Something went wrong: " + errorInfo.Info);
    }

    [PunRPC]
    void NewPlayerJoined()
    {
        _totalPlayers += 1;
    }
}
