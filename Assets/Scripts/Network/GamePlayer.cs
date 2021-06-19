using CarPlayer;
using Photon.Realtime;

namespace Network
{
    public struct GamePlayer
    {
        public int ID { get; }
        public int ActorNumber  { get; }
        public CarColors Color { get; }
        
        public GamePlayer(int id, int actorNumber, CarColors color)
        {
            ID = id;
            ActorNumber = actorNumber;
            Color = color;
        }
    }
}