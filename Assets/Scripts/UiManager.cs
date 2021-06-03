using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    public Slider fuelBar;    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fuelBar.minValue = 0;
        fuelBar.value = fuelBar.minValue;
    }

    //private void Update()
    //{
        //TODO removing call to fuel control until it is implemented correctly
        //FuelControl();
    //}

    //get fuel in the correct way
    // private void FuelControl()
    // {
    //     fuelBar.value = PlayerCar.Instance.fuelCar;
    //     if (fuelBar.value <= 0)
    //     {
    //         PlayerCar.Instance.statusPlayer = StatusCar.Broken;            
    //     }        
    // }
}
