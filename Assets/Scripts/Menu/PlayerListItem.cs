using System;
using CarPlayer;
using Network;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class PlayerListItem : MonoBehaviour
    {
        public TMP_Text text;
        private Player _player;

        private void Start()
        {
            GameRoom.Instance.ColorChanged += ColorChanged;
            GameConnection.Instance.OnPhotonPlayerLeftRoom += OnPhotonPlayerLeftRoom;
        }

        private void OnDestroy()
        {
            GameRoom.Instance.ColorChanged -= ColorChanged;
            GameConnection.Instance.OnPhotonPlayerLeftRoom -= OnPhotonPlayerLeftRoom;
        }

        private void OnDisable()
        {
            Destroy(gameObject);
        }

        private void ColorChanged(int actorNumber, int color)
        {
            print("request of color change " + actorNumber + color + " actual player " + _player);
            if (actorNumber == _player.ActorNumber)
            {
                float col = (float) color / 10;
                print("Changing color for player: " + actorNumber + col);
                text.color = Color.HSVToRGB(col, 1f, 1f);
            }
        }

        public void Setup(Player player)
        {
            _player = player;
            text.text = player.NickName;
        }

        private void OnPhotonPlayerLeftRoom(Player otherPlayer)
        {
            print("Player has left room " + otherPlayer + " current player " + _player);
            if (_player.Equals(otherPlayer))
            {
                Destroy(gameObject);
            }
        }
    }
}