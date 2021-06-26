using System;
using Network;
using TMPro;
using UnityEngine;

namespace Menu.Screens
{
    public class ResultsPanel : MonoBehaviour    
    {
        public TMP_Text resultsText;
        
        private void Start()
        {
            //TODO not working
            GameRoom.Instance.GameFinished += GameFinished;
        }


        private void GameFinished()
        {
            print("game finished, testing");
            gameObject.SetActive(true);
            resultsText.text = "";
            foreach (var gameResult in GameRoom.Instance.Results)
            {
                resultsText.text += $"#{gameResult.Position} {gameResult.Player}";
            }
        }
    }
}