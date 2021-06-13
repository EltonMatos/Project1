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
            if (pv.IsMine)
            {
                PhotonNetwork.Instantiate("Player", pos[0].position, pos[0].rotation);
            }
        }
    }
}