using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public enum StatusRace
{
    LobbyRoom,
    PreparingGame,
    PauseRace,
    FinishRace,
    StartRace
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public StatusRace race;    

    public float lapsMax;         

    public float timer;    
    public float timerRace;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        timer = 3;
        lapsMax = 0;        
    }    

    private void Update()
    {
        if(race == StatusRace.PreparingGame && CurrentScene.instance.phase != 0)
        {
            UiManager.Instance.timerStartRaceText.enabled = true;
            StartRacerTimer();
        }              
    }

    private void StartRacerTimer()
    {        
        timer -= Time.deltaTime;
        timerRace = Mathf.Round(timer);
        UiManager.Instance.timerStartRaceText.text = timerRace.ToString();
        if (timerRace == 0)
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
        timer = 3;
        race = StatusRace.StartRace;        
    }

    IEnumerator PreparingGame()
    {        
        yield return new WaitForSeconds(1);        
        UiManager.Instance.timerStartRaceText.enabled = true;        
        StartRacerTimer();        
    }
}
 