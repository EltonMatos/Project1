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

    public Camera mainCamera, cameraOne, cameraTwo;      

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
            //StartCoroutine(PreparingGame());
            UiManager.Instance.timerStartRaceText.enabled = true;
            StartRacerTimer();
        } 
        if(race == StatusRace.FinishRace)
        {
            //UiManager.Instance.TimerStartRace.text = "Fished Race";
        }
    }

    private void StartRacerTimer()
    {        
        timer -= Time.deltaTime;
        timerRace = Mathf.Round(timer);
        if(timerRace == 0)
        {
            UiManager.Instance.timerStartRaceText.text = "GO";            
            StartCoroutine(GoRace());
            return;
        }        
    }

    IEnumerator GoRace()
    {        
        yield return new WaitForSeconds(1);
        UiManager.Instance.timerStartRaceText.enabled = false;        
        race = StatusRace.StartRace;        
    }

    IEnumerator PreparingGame()
    {        
        yield return new WaitForSeconds(1);        
        UiManager.Instance.timerStartRaceText.enabled = true;        
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
 