using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;
using Photon.Pun;

public class CarManager : MonoBehaviour
{
    public PhotonView photonView;
    private PlayerCar player;

    private bool _isRunning = true;

    public float timer = 0;
    public bool checkLap;
    public string timePlayer;
    private string _showTimePlayer;

    private List<string> _listTimeLaps;

    public int completedLaps = 0;

    public int positionCar;
    public int idCarPosCurrent = 0;
    public int idCarPosPrevius = 0;

    public bool wrongWay;

    private void Start()
    {
        player = GetComponent<PlayerCar>();
        _listTimeLaps = new List<string>();        
        positionCar = 0;
        wrongWay = false;
        StartListTime();
    }

    private void Update()
    {
        if (completedLaps != GameManager.Instance.lapsMax) UpdateRacerTimer();

        FinishRace();

        if (photonView.IsMine)
        {
            UiManager.Instance.positionCarText.text = positionCar.ToString() + "º";
            /*UiManager.Instance.timerLap1CarText.text = listTimeLaps[0].ToString();
            UiManager.Instance.timerLap2CarText.text = listTimeLaps[1].ToString();
            UiManager.Instance.timerLap3CarText.text = listTimeLaps[2].ToString();*/
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

    private void StartListTime()
    {
        string time = "0.000";
        _listTimeLaps.Add(time);
        _listTimeLaps.Add(time);
        _listTimeLaps.Add(time);
    }

    public void AddLaps()
    {
        print("timer " + _listTimeLaps[0]);

        _showTimePlayer = "Lap: " + completedLaps + " - " + timePlayer;
        for (int i = 0; i < _listTimeLaps.Count; i++)
        {
            _listTimeLaps[i] = _showTimePlayer;
            print("timer " + _listTimeLaps[i]);
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
            player.audioCar.Stop();
            AudioManager.instance.audioS.Stop();
        }
    }

    IEnumerator FinishedRacer()
    {
        GameRoom.Instance.PlayerFinished(photonView.Owner);
        yield return new WaitForSeconds(2);
        player.statusPlayer = StatusCar.FinishedRace;
    }

    IEnumerator LastLapRacer()
    {        
        UiManager.Instance.lastLapText.enabled = true;
        yield return new WaitForSeconds(2);
        UiManager.Instance.lastLapText.enabled = false;
        UiManager.Instance.lastaLap = false;
    }
}