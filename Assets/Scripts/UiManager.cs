using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public Slider fuelBar;    

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        fuelBar.minValue = 0;
        fuelBar.value = fuelBar.minValue;
    }

    // Update is called once per frame
    void Update()
    {
        FuelControl();
    }

    private void FuelControl()
    {
        fuelBar.value = PlayerCar.instance.fuelCar;
        if (fuelBar.value <= 0)
        {
            PlayerCar.instance.statusPlayer = StatusCar.Broken;            
        }        
    }
}
