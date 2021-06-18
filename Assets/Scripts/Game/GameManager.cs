using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public enum StatusRace
{
    PreparingGame,
    PauseRace,
    FinishRace,
    StartRace
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public StatusRace race;

    public int lapsMax;

    //public Camera mainCamera, cameraOne, cameraTwo;  

    private bool _isRunning = true;

    public float timer = 3;    
    public float timerRace;   


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        lapsMax = 2;  
    }    

    private void Update()
    {
        if(race == StatusRace.PreparingGame)
        {
            StartCoroutine(PreparingGame());
        } 
        if(race == StatusRace.FinishRace)
        {
            //UiManager.Instance.TimerStartRace.text = "Fished Race";
        }
    }

    private void StartRacerTimer()
    {
        UiManager.Instance.TimerStartRace.enabled = false;
        timer -= Time.deltaTime;
        timerRace = Mathf.Round(timer);
        if(timerRace == 0)
        {
            UiManager.Instance.TimerStartRace.text = "GO";                        
            StartCoroutine(GoRace());
            return;
        }        
    }

    IEnumerator GoRace()
    {        
        yield return new WaitForSeconds(0.3f);
        UiManager.Instance.TimerStartRace.text = "";
        race = StatusRace.StartRace;
        
    }

    IEnumerator PreparingGame()
    {        
        yield return new WaitForSeconds(1);
        UiManager.Instance.TimerStartRace.enabled = true;
        StartRacerTimer();
    }

    public void TypeCamera()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Camera.main = cameraTwo;
        }
    }
}
 