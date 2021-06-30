using System.Collections.Generic;
using System.Linq;
using Game;
using Network;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace ResultsAndMenu
{
    public class ResultsPanel : MonoBehaviour
    {
        public TMP_Text resultsText;
        public GameObject goToRoomButton;

        private void OnEnable()
        {
            GameConnection.Instance.OnPhotonMasterClientSwitched += OnPhotonMasterClientSwitched;
            goToRoomButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        private void OnDisable()
        {
            GameConnection.Instance.OnPhotonMasterClientSwitched -= OnPhotonMasterClientSwitched;
        }

        private void OnPhotonMasterClientSwitched()
        {
            goToRoomButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public void GoToRoom()
        {
            PhotonNetwork.LoadLevel("Post Race Menu");
        }

        private void GameFinished()
        {
            List<GameResult> resultsHelpers = GameRoom.Instance.Results;
            resultsHelpers = resultsHelpers.OrderByDescending(o => o.TotalTime).ToList();
            resultsText.text = "";
            for (int i = 0; i < resultsHelpers.Count; i++)
            {
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (player.ActorNumber == resultsHelpers[i].Player.ActorNumber)
                    {
                        resultsText.text +=
                            $"Total time {resultsHelpers[i].TotalTime} #{i + 1} - {resultsHelpers[i].Player.Color} - {player.NickName} - " +
                            $"PB: Lap {resultsHelpers[i].Player.BestLap} {resultsHelpers[i].Player.BestTime}\n\n";
                    }
                }
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            GameFinished();
        }
    }
}