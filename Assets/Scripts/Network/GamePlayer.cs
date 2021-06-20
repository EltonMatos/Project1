using CarPlayer;
using Photon.Realtime;

namespace Network
{
    public struct GamePlayer
    {
        public int ID { get; }
        public int ActorNumber  { get; }
        public CarColors Color { get; private set; }
        
        public GamePlayer(int id, int actorNumber, CarColors color)
        {
            ID = id;
            ActorNumber = actorNumber;
            Color = color;
        }

        public void SetColor(CarColors color)
        {
            Color = color;
        }

        public override string ToString()
        {
            return $"Id {ID} Actor {ActorNumber} Color {(int) Color} {Color}";
        }
    }
}