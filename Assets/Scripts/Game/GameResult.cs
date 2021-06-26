using Network;

namespace Game
{
    public struct GameResult
    {
        public GamePlayer Player { get; private set; }
        public int Position { get; set; }

        public GameResult(GamePlayer player, int position)
        {
            Player = player;
            Position = position;
        }

    }
}