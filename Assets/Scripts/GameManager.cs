using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isRunning = true;
        
    public float timer = 0;
    public bool checkLap;
    public bool checkOldLap;
    public float timePlayer;
    
    public int lapsMax;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        checkLap = false;
        checkOldLap = false;        
        lapsMax = 10;
    }

    // Update is called once per frame
    void Update()
    {
        cronoMark();
    }

    void cronoMark()
    {
        if (isRunning && checkLap)
        {
            timer += Time.deltaTime;            
            timePlayer = Mathf.Round(timer);

            /*if (checkLap)
            {
                isRunning = false;
                checkLap = false;
                timePlayer = "0";                
            }*/
        }        
    }

    public float returnTime()
    {
        return timePlayer;
    }
}
