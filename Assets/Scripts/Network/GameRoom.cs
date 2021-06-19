using System;
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

        public PhotonView PhotonView;
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
                //TODO get correct color and id for player
                int gameId = GetAvailableId();
                int color = GetAvailableColor();
                PhotonView.RPC("NewPlayer", RpcTarget.AllBufferedViaServer, gameId, newPlayer.ActorNumber, color);
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
            
            print("Assigning id " + id);
            return id;
        }
        
        private int GetAvailableColor()
        {
            if (_players.Count == 0) return 1;

            int id = 0;
            //this starts search at index 1 as the 0 is none
            for (int i = 1; i <= _players.Count + 1; i++)
            {
                bool colorAvailable = true;
                foreach (GamePlayer player in _players)
                {
                    if (player.Color == (CarColors) i)
                    {
                        colorAvailable = false;
                        break;
                    }
                }
                if (colorAvailable)
                {
                    id = i;
                    break;
                }
            }
            
            print("Assigning color " + id);
            return id;
        }

        private void RemovePlayer(Player player)
        {
            print("removing player from list, " + player.ActorNumber);
            foreach (GamePlayer p in _players)
            {
                if (p.ActorNumber == player.ActorNumber)
                {
                    _players.Remove(p);
                    break;
                }
            }

            //if (PhotonNetwork.IsMasterClient)
            //{
            //    PhotonView.RPC("RemovePlayer", RpcTarget.AllBufferedViaServer, player.ActorNumber);
            //}
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

        
        [PunRPC]
        public void NewPlayer(int gameId, int actorNumber, int carColor)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.ActorNumber == actorNumber)
                {
                    print("adding player to list, " + gameId + actorNumber + (CarColors) carColor);
                    GamePlayer newGamePlayer = new GamePlayer(gameId, actorNumber, (CarColors) carColor);
                    _players.Add(newGamePlayer);
                    ColorChanged?.Invoke(actorNumber, carColor);
                }
            }
        }
    }
}