using System;
using System.Collections;
using System.Collections.Generic;
using CarPlayer;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class GameRoom : MonoBehaviour
    {
        public static GameRoom Instance;

        public PhotonView photonView;
        private List<GamePlayer> _players = new List<GamePlayer>();
        public Action<int, int> ColorChanged;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            GameConnection.Instance.OnPhotonPlayerLeftRoom += RemovePlayer;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name.Contains("Track"))
            {
                var transform1 = transform;
                PhotonNetwork.Instantiate("PhotonNetworkPlayer", transform1.position, transform1.rotation);
            }
        }

        public void ResetRoom()
        {
            _players = new List<GamePlayer>();
        }

        public void AddPlayer(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int gameId = GetAvailableId();
                int color = GetNextAvailableColor((int) CarColors.None);
                photonView.RPC("NewPlayer", RpcTarget.AllBufferedViaServer, gameId, newPlayer.ActorNumber, color);
            }
        }

        private int GetAvailableId()
        {
            if (_players.Count == 0) return 0;

            int id = 999;
            for (int i = 0; i <= _players.Count; i++)
            {
                bool idAvailable = true;
                foreach (GamePlayer player in _players)
                {
                    if (player.ID == i)
                    {
                        idAvailable = false;
                        break;
                    }
                }

                if (idAvailable)
                {
                    id = i;
                    break;
                }
            }

            return id;
        }

        private int GetNextAvailableColor(int current)
        {
            int id = 0;
            for (int i = current; i < Enum.GetNames(typeof(CarColors)).Length + current; i++)
            {
                int colorToCheck = i > Enum.GetNames(typeof(CarColors)).Length
                    ? i - Enum.GetNames(typeof(CarColors)).Length
                    : i;
                if (colorToCheck == (int) CarColors.None) continue;

                bool colorAvailable = true;
                foreach (GamePlayer player in _players)
                {
                    if (player.Color == (CarColors) colorToCheck)
                    {
                        colorAvailable = false;
                        break;
                    }
                }

                if (colorAvailable)
                {
                    id = colorToCheck;
                    break;
                }
            }

            return id;
        }

        private void RemovePlayer(Player player)
        {
            foreach (GamePlayer p in _players)
            {
                if (p.ActorNumber == player.ActorNumber)
                {
                    _players.Remove(p);
                    break;
                }
            }
        }

        public int GetId(Player localPlayer)
        {
            //initialized with non existent ID
            int id = 999;
            foreach (GamePlayer photonPlayer in _players)
            {
                if (photonPlayer.ActorNumber == localPlayer.ActorNumber)
                {
                    id = photonPlayer.ID;
                }
            }

            return id;
        }

        public void ChangeCurrentPlayerColor()
        {
            photonView.RPC("RequestColorChange", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
        }


        [PunRPC]
        public void NewPlayer(int gameId, int actorNumber, int carColor)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.ActorNumber == actorNumber)
                {
                    GamePlayer newGamePlayer = new GamePlayer(gameId, actorNumber, (CarColors) carColor);
                    _players.Add(newGamePlayer);
                    StartCoroutine(InvokeChangeColorEvent(actorNumber, carColor));
                }
            }
        }

        [PunRPC]
        public void RequestColorChange(int actorNumber)
        {
            int currentColor = 0;
            foreach (GamePlayer player in _players)
            {
                if (player.ActorNumber == actorNumber)
                {
                    currentColor = (int) player.Color;
                    break;
                }
            }

            int newColor = GetNextAvailableColor(currentColor);
            photonView.RPC("ChangeColorForActor", RpcTarget.AllBufferedViaServer, actorNumber, newColor);
        }

        [PunRPC]
        public void ChangeColorForActor(int actorNumber, int color)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].ActorNumber == actorNumber)
                {
                    _players[i] = new GamePlayer(_players[i].ID, _players[i].ActorNumber, (CarColors) color);
                    StartCoroutine(InvokeChangeColorEvent(actorNumber, color));
                }
            }
        }

        private IEnumerator InvokeChangeColorEvent(int actorNumber, int carColor)
        {
            yield return new WaitForSeconds(0.5f);
            ColorChanged?.Invoke(actorNumber, carColor);
        }
    }
}