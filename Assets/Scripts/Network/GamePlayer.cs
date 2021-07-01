using CarPlayer;
using Photon.Realtime;
using UnityEngine;

namespace Network
{
    public struct GamePlayer
    {
        public int ID { get; }
        public int ActorNumber  { get; }
        public CarColors Color { get; private set; }
        public float BestTime { get; private set; }
        public int BestLap { get; private set; }
        public int CompletedLaps { get; private set; }
        
        public GamePlayer(int id, int actorNumber, CarColors color)
        {
            ID = id;
            ActorNumber = actorNumber;
            Color = color;
            BestTime = float.MaxValue;
            BestLap = int.MaxValue;
            CompletedLaps = 0;
        }
        
        public GamePlayer(int id, int actorNumber, CarColors color, string time, int lap, int completedLaps)
        {
            ID = id;
            ActorNumber = actorNumber;
            Color = color;
            BestTime = float.Parse(time);
            BestLap = lap;
            CompletedLaps = completedLaps;
        }

        public void SetColor(CarColors color)
        {
            Color = color;
        }
        
        public bool TryInsertTimeAndLap(string lapTime, int lapNum)
        {
            float floatTimeLap = float.Parse(lapTime);
            if (floatTimeLap < BestTime)
            {
                BestTime = floatTimeLap;
                BestLap = lapNum;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"Id {ID} Actor {ActorNumber} Color {(int) Color} {Color} BL {BestLap} PB {BestTime} CL {CompletedLaps}";
        }
    }
}