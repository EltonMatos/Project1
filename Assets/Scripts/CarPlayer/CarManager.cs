using System.Collections;
using System.Collections.Generic;
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
    private string showTimePlayer;

    private List<string> listTimeLaps;

    public int lapsMax;
    public int completedLaps = 0;

    public int positionCar;
    public int idCarPosCurrent = 0;
    public int idCarPosPrevius = 0;

    private void Start()
    {
        player = GetComponent<PlayerCar>();
        listTimeLaps = new List<string>();
        lapsMax = GameManager.Instance.lapsMax;
        positionCar = 0;
        StartListTime();
    }

    private void Update()
    {
        UpdateRacerTimer();        
        FinishRace();
        if (photonView.IsMine)
        {
            UiManager.Instance.positionCarText.text = positionCar.ToString();
            /*UiManager.Instance.timerLap1CarText.text = listTimeLaps[0].ToString();
            UiManager.Instance.timerLap2CarText.text = listTimeLaps[1].ToString();
            UiManager.Instance.timerLap3CarText.text = listTimeLaps[2].ToString();*/

            if (UiManager.Instance.wrongWayShow)
            {
                UiManager.Instance.wrongWayText.enabled = true;
            }
            else UiManager.Instance.wrongWayText.enabled = false;
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
        listTimeLaps.Add(time);
        listTimeLaps.Add(time);
        listTimeLaps.Add(time);
    }

    public void AddLaps(int lap)
    {
        print("timer " + listTimeLaps[0].ToString());

        showTimePlayer = "Lap: " + lap + " - " + timePlayer;
        for(int i = 0; i < listTimeLaps.Count; i++)
        {
            listTimeLaps[i] = showTimePlayer;
            print("timer " + listTimeLaps[i].ToString());
        }
    }

    public void FinishRace()
    {
        if (completedLaps == GameManager.Instance.lapsMax)
        {
            print("Last Lap");
        }
        if (completedLaps > GameManager.Instance.lapsMax)
        {
            GameManager.Instance.race = StatusRace.FinishRace;
            StartCoroutine(FinishedRacer());
        }
    }

    IEnumerator FinishedRacer()
    {
        yield return new WaitForSeconds(2);
        player.statusPlayer = StatusCar.FinishedRace;
    }    
}
