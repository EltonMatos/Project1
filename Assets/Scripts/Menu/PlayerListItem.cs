using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class PlayerListItem : MonoBehaviourPunCallbacks
    {
        public TMP_Text text;
        private Player _player;
        
        public void Setup(Player player)
        {
            _player = player;
            text.text = player.NickName;
            text.color = Color.HSVToRGB(Random.Range(0f, 1f), 1f, 1f);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (_player.Equals(otherPlayer))
            {
                Destroy(gameObject);
            }
        }

        public override void OnLeftRoom()
        {
            Destroy(gameObject);
        }
    }
}
