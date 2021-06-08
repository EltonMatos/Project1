using System;
using Network;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Menu.Screens
{
    public class LobbyMenu : Menu
    {
        public TMP_Text nicknameText;

        private void OnEnable()
        {
            nicknameText.text = PhotonNetwork.NickName;
        }

        public void CreateOrJoinRoom(TMP_InputField roomInput)
        {
            GameConnection.Instance.CreateOrJoinRoom(roomInput.text);
        }
    }
}