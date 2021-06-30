using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
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
        public List<GameResult> Results = new List<GameResult>();
        public Action<int, int> ColorChanged;
        public Action GameFinished;

        private void Awake()
        {
            //TODO have this working properly creating the Photon View so menus can be unified
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            GameConnection.Instance.OnPhotonPlayerLeftRoom += RemovePlayer;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            GameConnection.Instance.OnPhotonPlayerLeftRoom -= RemovePlayer;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name.Contains("Track"))
            {
                var transform1 = transform;
                PhotonNetwork.Instantiate("PhotonNetworkPlayer", transform1.position, transform1.rotation);
            }

            if (scene.name.Contains("Menu"))
            {
                print("reseting game results");
                Results.Clear();
            }
        }

        public void ResetRoom()
        {
            _players = new List<GamePlayer>();
            Results = new List<GameResult>();
        }

        public void AddPlayer(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int gameId = GetAvailableId(newPlayer.ActorNumber);
                int color = GetNextAvailableColor((int) CarColors.None, newPlayer.ActorNumber);
                photonView.RPC("NewPlayer", RpcTarget.AllBufferedViaServer, gameId, newPlayer.ActorNumber, color);
            }
        }

        private int GetAvailableId(int actorNumber)
        {
            if (_players.Count == 0) return 0;

            int id = 999;
            for (int i = 0; i <= _players.Count; i++)
            {
                bool idAvailable = true;
                foreach (GamePlayer player in _players)
                {
                    if (player.ActorNumber == actorNumber)
                    {
                        return player.ID;
                    }

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

        private int GetNextAvailableColor(int current, int actorNumber)
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
                    if (player.ActorNumber == actorNumber)
                    {
                        return (int) player.Color;
                    }

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

        public void AddLapTime(string time, int lap)
        {
            photonView.RPC("AddPersonalLapTime", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber, time,
                lap);
        }

        public void PlayerFinished(Player photonViewOwner, float totalTime)
        {
            photonView.RPC("SetPlayerFinished", RpcTarget.MasterClient, photonViewOwner.ActorNumber, totalTime);
        }

        public void UpdateNumberOfLaps(float num)
        {
            photonView.RPC("UpdateNumLaps", RpcTarget.OthersBuffered, num);
        }
        
        [PunRPC]
        public void AddPersonalLapTime(int actorNumber, string time, int lapNum)
        {
            foreach (GamePlayer gamePlayer in _players)
            {
                if (gamePlayer.ActorNumber == actorNumber)
                {
                    bool newBest = gamePlayer.TryInsertTimeAndLap(time, lapNum);
                    if (newBest)
                    {
                        photonView.RPC("UpdateBestPersonalForPlayer", RpcTarget.AllViaServer, actorNumber, time,
                            lapNum);
                    }
                }
            }
        }

        [PunRPC]
        public void UpdateBestPersonalForPlayer(int actorNumber, string time, int lapNum)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].ActorNumber == actorNumber)
                {
                    _players[i] = new GamePlayer(_players[i].ID, _players[i].ActorNumber, _players[i].Color, time,
                        lapNum);
                }
            }
        }

        [PunRPC]
        public void SetPlayerFinished(int actorNumber, float time)
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
                        StartCoroutine(CheckIfLapDataIsLoadAndAddResult(actorNumber, gamePlayer, time, Results.Count + 1,
                            true));
                        break;
                    }
                }
            }
        }

        [PunRPC]
        public void UpdateResultsList(int actorNumber, float time, int position)
        {
            foreach (GamePlayer gamePlayer in _players)
            {
                if (gamePlayer.ActorNumber == actorNumber)
                {
                    print($"Adding #{position} {gamePlayer}");
                    //Results.Add(new GameResult(gamePlayer, position));
                    StartCoroutine(CheckIfLapDataIsLoadAndAddResult(actorNumber, gamePlayer, time, position));
                }
            }
        }

        [PunRPC]
        public void GameHasFinished()
        {
            print("game has finished received rpc");
            //GameFinished?.Invoke();

            StartCoroutine(CheckIfLapDataIsLoadAndFinishGame());
        }

        [PunRPC]
        public void NewPlayer(int gameId, int actorNumber, int carColor)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.ActorNumber == actorNumber)
                {
                    bool playerExists = false;
                    for (int i = 0; i < _players.Count; i++)
                    {
                        if (_players[i].ActorNumber == actorNumber)
                        {
                            playerExists = true;
                            _players[i].SetColor((CarColors) carColor);
                            break;
                        }
                    }

                    if (!playerExists)
                    {
                        GamePlayer newGamePlayer = new GamePlayer(gameId, actorNumber, (CarColors) carColor);
                        _players.Add(newGamePlayer);
                    }

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

            int newColor = GetNextAvailableColor(currentColor, int.MaxValue);
            photonView.RPC("ChangeColorForActor", RpcTarget.AllBufferedViaServer, actorNumber, newColor);
        }
        
        [PunRPC]
        public void UpdateNumLaps(float num)
        {
            GameManager.Instance.lapsMax = num;
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

        private IEnumerator CheckIfLapDataIsLoadAndAddResult(int actorNumber, GamePlayer gamePlayer, float time, int position,
            bool master = false, int called = 0)
        {
            bool dataLoaded = GamePlayerResultDataCheck();

            yield return new WaitForSeconds(0.1f);
            if (dataLoaded)
            {
                bool resultForPlayerExists = false;
                foreach (GameResult player in Results)
                {
                    if (player.Player.ActorNumber == actorNumber)
                    {
                        resultForPlayerExists = true;
                    }
                }

                if (!resultForPlayerExists)
                {
                    GameResult gameResult = new GameResult(gamePlayer, time);
                    Results.Add(gameResult);
                    if (master)
                    {
                        photonView.RPC("UpdateResultsList", RpcTarget.Others, actorNumber, time, position);
                        if (_players.Count == Results.Count)
                        {
                            photonView.RPC("GameHasFinished", RpcTarget.AllViaServer);
                        }
                    }
                }
            }
            else if (called < 100)
            {
                StartCoroutine(CheckIfLapDataIsLoadAndAddResult(actorNumber, gamePlayer, time, position, master, called + 1));
            }
            else
            {
                print("failed to load result, should call the disconnect and failure method");
            }
        }

        private IEnumerator CheckIfLapDataIsLoadAndFinishGame(int called = 0)
        {
            bool dataLoaded = GamePlayerResultDataCheck();

            yield return new WaitForSeconds(0.1f);
            if (dataLoaded)
            {
                GameFinished?.Invoke();
            }
            else if (called < 100)
            {
                StartCoroutine(CheckIfLapDataIsLoadAndFinishGame(called + 1));
            }
            else
            {
                print("failed to load result, should call the disconnect and failure method");
            }
        }

        private bool GamePlayerResultDataCheck()
        {
            foreach (GamePlayer player in _players)
            {
                if (player.BestLap > 100)
                {
                    return false;
                }
            }

            return true;
        }
    }
}