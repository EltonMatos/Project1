using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    private PlayerCar player;

    private bool _isRunning = true;

    public float timer = 0;
    public bool checkLap;    
    public float timePlayer;

    private List<float> listTimeLaps;

    public int lapsMax;
    public int completedLaps;

    private void Start()
    {
        player = GetComponent<PlayerCar>();
        listTimeLaps = new List<float>();
    }

    private void Update()
    {
        UpdateRacerTimer();
        ListTimes();
        FinishRace();
    }

    private void UpdateRacerTimer()
    {
        if (_isRunning && checkLap)
        {
            timer += Time.deltaTime;
            timePlayer = Mathf.Round(timer);            
        }
    }

    public float ReturnTime()
    {
        return timePlayer;
    }

    public void AddLaps(int lap)
    {
        listTimeLaps.Add(timePlayer);        
    }

    public void FinishRace()
    {
        if (completedLaps == GameManager.Instance.lapsMax)
        {
            print("Ultima volta");
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
        for(int i = 0; i < listTimeLaps.Count; i++)
        {
            Debug.Log("Lap: " + completedLaps + " Time: " + listTimeLaps[i].ToString());
        }
    }
}
