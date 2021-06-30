using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;


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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Track"))
        {
            timer = 3;
            race = StatusRace.PreparingGame;
        }
    }

    private void Update()
    {
        if (race == StatusRace.PreparingGame && SceneManager.GetActiveScene().name.Contains("Track"))
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
        }
    }

    IEnumerator GoRace()
    {
        yield return new WaitForSeconds(1);
        UiManager.Instance.timerStartRaceText.enabled = false;
        timer = 3;
        race = StatusRace.StartRace;
    }
}