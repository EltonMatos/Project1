using System;
using Game;
using Photon.Pun;
using UnityEngine;

namespace Network
{
    public class PhotonNetworkPlayer : MonoBehaviour
    {
        [SerializeField]
        private PhotonView pv;
        
        private void OnEnable()
        {
            var pos = GameSetup.Instance.gridPositions;
            int playerRoomId = GameRoom.Instance.GetId(PhotonNetwork.LocalPlayer);
            if (pv.IsMine && playerRoomId < 999)
            {
                PhotonNetwork.Instantiate("Player", pos[playerRoomId].position, pos[playerRoomId].rotation);
            }
        }
    }
}