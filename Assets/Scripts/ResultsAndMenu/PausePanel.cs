using System;
using Network;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace ResultsAndMenu
{
    public class PausePanel : MonoBehaviour
    {
        public GameObject pausePanel;
        public GameObject pausedPanel;

        private bool _paused = false;
        
        private void Start()
        {
            _paused = false;
            TogglePanels();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _paused = !_paused;
                TogglePanels();
            }
        }

        private void TogglePanels()
        {
            if (_paused)
            {
                pausePanel.SetActive(false);
                pausedPanel.SetActive(true);
            }
            else
            {
                pausePanel.SetActive(true);
                pausedPanel.SetActive(false);
            }
        }
        
        public void Pause()
        {
            _paused = true;
            TogglePanels();
        }

        public void Unpause()
        {
            _paused = false;
            TogglePanels();
        }

        public void QuitRace()
        {
            print("Quitting race");
            PhotonNetwork.Disconnect();
        }
    }
}