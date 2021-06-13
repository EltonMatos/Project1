using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class PhotonRoom : MonoBehaviour
    {
        public static PhotonRoom Instance;
        
        public List<PhotonPlayer> players =  new List<PhotonPlayer>();
        
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
            PhotonPlayer newPhotonPlayer = new PhotonPlayer(players.Count, newPlayer);
            players.Add(newPhotonPlayer);
        }
        
        public void RemovePlayer(Player newPlayer)
        {
            throw new NotImplementedException();
        }

        public int GetId(Player localPlayer)
        {
            //initialized with non existent ID
            int id = 999;
            foreach (PhotonPlayer photonPlayer in players)
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