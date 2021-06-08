using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool _isRunning = true;
        
    public float timer = 0;
    public bool checkLap;
    public bool checkOldLap;
    public float timePlayer;
    
    public int lapsMax;
    public int completedLaps;

    public Camera mainCamera, cameraOne, cameraTwo;

    public GameObject playerCar;
    public Transform[] carPositions;   


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        checkLap = false;
        checkOldLap = false;        
        lapsMax = 3;
        completedLaps = -1;        
        
        CreateCar();
    }

    private void CreateCar()
    {
        //instantiate cars
    }

    private void Update()
    {
        UpdateStopwatch();
    }

    private void UpdateStopwatch()
    {
        if (_isRunning && checkLap)
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

    public float ReturnTime()
    {
        return timePlayer;
    }

    public void TypeCamera()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Camera.main = cameraTwo;
        }
    }

    public void FinishRace()
    {
        if(completedLaps > lapsMax)
        {
            print("Corrida terminou");
        }
    }
}
 