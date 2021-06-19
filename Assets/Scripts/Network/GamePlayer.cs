using Photon.Realtime;

namespace Network
{
    public struct GamePlayer
    {
        public int ID { get; }
        public Player Player  { get; }
        
        public GamePlayer(int id, Player player)
        {
            ID = id;
            Player = player;
        }
    }
}