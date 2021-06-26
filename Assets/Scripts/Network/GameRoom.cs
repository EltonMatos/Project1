using System;
using System.Collections;
using System.Collections.Generic;
using CarPlayer;
using Game;
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
        public readonly List<GameResult> Results = new List<GameResult>();
        public Action<int, int> ColorChanged;
        public Action GameFinished;

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
                int colorToCheck = i >= Enum.GetNames(typeof(CarColors)).Length
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
        
        public CarColors GetColor(Player localPlayer)
        {
            CarColors color = CarColors.None;
            foreach (GamePlayer photonPlayer in _players)
            {
                if (photonPlayer.ActorNumber == localPlayer.ActorNumber)
                {
                    color = photonPlayer.Color;
                }
            }

            return color;
        }
        
        public CarColors GetColorByLocalGameId(int id)
        {
            CarColors color = CarColors.None;
            foreach (GamePlayer photonPlayer in _players)
            {
                if (photonPlayer.ID == id)
                {
                    color = photonPlayer.Color;
                }
            }

            return color;
        }

        public void ChangeCurrentPlayerColor()
        {
            photonView.RPC("RequestColorChange", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        
        public void PlayerFinished(Player photonViewOwner)
        {
            photonView.RPC("SetPlayerFinished", RpcTarget.MasterClient, photonViewOwner.ActorNumber);
        }
        
        [PunRPC]
        public void SetPlayerFinished(int actorNumber)
        {
            bool playerFound = false;
            foreach (GameResult gameResult in Results)
            {
                if (gameResult.Player.ActorNumber == actorNumber)
                {
                    playerFound = true;
                } 
            }

            if (!playerFound)
            {
                foreach (GamePlayer gamePlayer in _players)
                {
                    if (gamePlayer.ActorNumber == actorNumber)
                    {
                        GameResult gameResult = new GameResult(gamePlayer, Results.Count + 1);
                        print($"Adding #{gameResult.Position} {gamePlayer}");
                        Results.Add(gameResult);
                        photonView.RPC("UpdateResultsList", RpcTarget.Others, actorNumber, gameResult.Position);
                        break;
                    } 
                }
            }
            
            print(_players.Count + " player : results " + Results.Count);
            if (_players.Count == Results.Count)
            {
                GameFinished?.Invoke();
            }

        }
        
        [PunRPC]
        public void UpdateResultsList(int actorNumber, int position)
        {
            foreach (GamePlayer gamePlayer in _players)
            {
                if (gamePlayer.ActorNumber == actorNumber)
                {
                    print($"Adding #{position} {gamePlayer}");
                    Results.Add(new GameResult(gamePlayer, position));
                } 
            }
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