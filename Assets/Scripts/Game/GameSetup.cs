using System;
using UnityEngine;

namespace Game
{
    public class GameSetup : MonoBehaviour
    {
        public static GameSetup Instance;

        public Transform[] gridPositions;

        private void Awake()
        {
            Instance = this;
        }
    }
}