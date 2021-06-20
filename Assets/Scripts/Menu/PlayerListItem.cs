using CarPlayer;
using Network;
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
            if (actorNumber == _player.ActorNumber)
            {
                text.color = CarColorManager.Instance.GetColor((CarColors) color);
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