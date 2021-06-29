using Network;

namespace Game
{
    public struct GameResult
    {
        public GamePlayer Player { get; private set; }
        public int Position { get; set; }

        public GameResult(GamePlayer player)
        {
            Player = player;
            Position = 0;
        }

    }
}