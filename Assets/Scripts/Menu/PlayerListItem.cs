using Photon.Pun;
using Photon.Realtime;
using TMPro;

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
