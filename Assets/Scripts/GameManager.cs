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
    public int completedLaps;

    public Camera mainCamera, cameraOne, cameraTwo;

    public GameObject playerCar;
    public Transform posCar1, posCar2, posCar3, posCar4, posCar5, posCar6;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        checkLap = false;
        checkOldLap = false;        
        lapsMax = 3;
        completedLaps = -1;        
        CreateCar();
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

    public void typeCamera()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Camera.main = cameraTwo;
        }
    }

    public void CreateCar()
    {
        GameObject pl = Instantiate(playerCar, posCar1.position, Quaternion.identity) as GameObject;
        pl.transform.Rotate(new Vector3(0, 180, 0),180f);
    }

    public void FinishRace()
    {
        if(completedLaps > lapsMax)
        {
            print("Corrida terminou");
        }
    }
}
 