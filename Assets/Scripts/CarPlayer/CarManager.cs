using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Network;
using UnityEngine;
using Photon.Pun;

public class CarManager : MonoBehaviour
{
    public PhotonView photonView;
    private PlayerCar _player;

    private bool _isRunning = true;

    public float timer;
    public bool checkLap;
    public string timePlayer;
    private string _showTimePlayer;

    private List<float> _listTimeLaps = new List<float>();

    public int completedLaps;

    public int positionCar;
    public int idCarPosCurrent;
    public int idCarPosPrevius;

    public bool wrongWay;

    private void OnEnable()
    {
        _listTimeLaps.Clear();
    }

    private void Start()
    {
        _player = GetComponent<PlayerCar>();
        _listTimeLaps.Clear();        
        positionCar = 0;
        wrongWay = false;
    }

    private void Update()
    {
        if (completedLaps != GameManager.Instance.lapsMax) UpdateRacerTimer();

        FinishRace();

        if (photonView.IsMine)
        {
            UiManager.Instance.positionCarText.text = positionCar + "º";
            UiManager.Instance.wrongWayText.enabled = wrongWay;
        }
    }

    private void UpdateRacerTimer()
    {
        if (_isRunning && checkLap)
        {
            timer += Time.deltaTime;
            timePlayer = timer.ToString("F4");
        }
    }

    public string ReturnTime()
    {
        return timePlayer;
    }

    public void AddLaps()
    {
        _listTimeLaps.Add(timer);

        if (photonView.IsMine)
        {
            GameRoom.Instance.AddLapTime(timePlayer, completedLaps);
        }
    }

    public void FinishRace()
    {
        if (completedLaps == GameManager.Instance.lapsMax - 1 && UiManager.Instance.lastaLap)
        {
            StartCoroutine(LastLapRacer());
        }

        if (completedLaps == GameManager.Instance.lapsMax)
        {
            StartCoroutine(FinishedRacer());
            _player.audioCar.Stop();
            AudioManager.instance.audioS.Stop();
        }
    }

    IEnumerator FinishedRacer()
    {
        if (photonView.IsMine)
        {
            float totalTime = 0;
            foreach (float timeLap in _listTimeLaps)
            {
                totalTime += timeLap;
            }
            GameRoom.Instance.PlayerFinished(photonView.Owner, totalTime);
        }
        yield return new WaitForSeconds(2);
        _player.statusPlayer = StatusCar.FinishedRace;
    }

    IEnumerator LastLapRacer()
    {        
        UiManager.Instance.lastLapText.enabled = true;
        yield return new WaitForSeconds(2);
        UiManager.Instance.lastLapText.enabled = false;
        UiManager.Instance.lastaLap = false;
    }
}