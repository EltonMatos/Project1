using System;
using Network;
using UnityEngine;

namespace ResultsAndMenu
{
    public class ResultsManager : MonoBehaviour
    {
        public ResultsPanel resultsPanel;
        
        private void Start()
        {
            GameRoom.Instance.GameFinished += GameFinished;
            resultsPanel.Close();
        }

        private void OnDestroy()
        { 
            GameRoom.Instance.GameFinished -= GameFinished;
        }

        private void GameFinished()
        {
            resultsPanel.Open();
        }
    }
}