using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isRunning = true;
        
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

    // Start is called before the first frame update
    private void Start()
    {
        checkLap = false;
        checkOldLap = false;        
        lapsMax = 3;
        completedLaps = -1;        
        //CreateCar();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateStopwatch();
    }

    private void UpdateStopwatch()
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

    //TODO this can be removed, it will be created at the network manager
    // private void CreateCar()
    // {
    //     GameObject pl = Instantiate(playerCar, posCar1.position, Quaternion.identity) as GameObject;
    //     pl.transform.Rotate(new Vector3(0, 180, 0),180f);
    // }

    public void FinishRace()
    {
        if(completedLaps > lapsMax)
        {
            print("Corrida terminou");
        }
    }
}
 