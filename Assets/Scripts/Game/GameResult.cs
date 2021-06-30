using Network;

namespace Game
{
    public struct GameResult
    {
        public GamePlayer Player { get; private set; }
        public float TotalTime { get; set; }

        public GameResult(GamePlayer player, float totalTime)
        {
            Player = player;
            TotalTime = totalTime;
        }

    }
}