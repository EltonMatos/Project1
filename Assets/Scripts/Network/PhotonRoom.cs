using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class PhotonRoom : MonoBehaviour
    {
        private readonly string _raceScene = "Track 1";
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == _raceScene)
            {
                var transform1 = transform;
                PhotonNetwork.Instantiate("PhotonNetworkPlayer", transform1.position, transform1.rotation);
            }
        }
    }
}