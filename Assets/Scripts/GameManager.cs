using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int lapsMax;

    public Camera mainCamera, cameraOne, cameraTwo;

    public GameObject[] playerCar;
    private PlayerCar player;
    //public Transform[] carPositions;   


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GetComponent<PlayerCar>();
        lapsMax = 2;  
    }    

    private void Update()
    {
        //PositionCarRace();
    }   

    public void AddCarRacer()
    {
        //adicionar players na corrida
    }

    public void TypeCamera()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Camera.main = cameraTwo;
        }
    }

    public void PositionCarRace()
    {
        for (int i = 0; i < playerCar.Length; i++)
        {
            Debug.Log("Position: " + i+1 + " Car number: " + player.idCar);
        }
    }

    
}
 