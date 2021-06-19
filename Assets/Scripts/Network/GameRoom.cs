using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class GameRoom : MonoBehaviour
    {
        public static GameRoom Instance;
        
        public List<GamePlayer> players =  new List<GamePlayer>();
        
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name.Contains("Track"))
            {
                var transform1 = transform;
                PhotonNetwork.Instantiate("PhotonNetworkPlayer", transform1.position, transform1.rotation);
            }
        }

        public void AddPlayer(Player newPlayer)
        {
            GamePlayer newGamePlayer = new GamePlayer(players.Count, newPlayer);
            players.Add(newGamePlayer);
        }
        
        public void RemovePlayer(Player newPlayer)
        {
            throw new NotImplementedException();
        }

        public int GetId(Player localPlayer)
        {
            //initialized with non existent ID
            int id = 999;
            foreach (GamePlayer photonPlayer in players)
            {
                if (photonPlayer.Player.UserId == localPlayer.UserId)
                {
                    id = photonPlayer.ID;
                }
            }
            return id;
        }
    }
}