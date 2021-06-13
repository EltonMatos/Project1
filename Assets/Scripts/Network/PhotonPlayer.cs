using Photon.Realtime;

namespace Network
{
    public struct PhotonPlayer
    {
        public int ID { get; }
        public Player Player  { get; }
        
        public PhotonPlayer(int id, Player player)
        {
            ID = id;
            Player = player;
        }
    }
}