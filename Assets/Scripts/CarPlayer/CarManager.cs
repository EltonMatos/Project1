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

    private void Start()
    {
        player = GetComponent<PlayerCar>();
        listTimeLaps = new List<string>();
        lapsMax = GameManager.Instance.lapsMax;
        positionCar = 0;
        
    }

    private void Update()
    {
        UpdateRacerTimer();
        //ListTimes();
        FinishRace();
        if (photonView.IsMine)
        {
            UiManager.Instance.positionCarText.text = positionCar.ToString();
            UiManager.Instance.timerLapCarText.text = timePlayer.ToString();
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

    public void AddLaps(int lap)
    {
        showTimePlayer = "Lap: " + lap + " - " + timePlayer;
        listTimeLaps.Add(showTimePlayer);
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

    void ListTimes()
    {       

        for (int i = 0; i < listTimeLaps.Count; i++)
        {            
            Debug.Log("Lap: " + completedLaps + " Time: " + listTimeLaps.ToString());
            UiManager.Instance.timerLapCarText.text = listTimeLaps[i].ToString();
        }
    }
}
