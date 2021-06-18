using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    public Slider fuelBar;
    public float sliderBarValue;

    public Text statusCar;
    public Text TimerStartRace;
    public Text FinishedRace;

    public Image[] imgTurbo;
    public int qntTurbo;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {        
        fuelBar.minValue = 0;
        fuelBar.value = fuelBar.minValue;
        qntTurbo = 0;
    }

    private void Update()
    {
        //TODO removing call to fuel control until it is implemented correctly
        FuelControl();
        if(GameManager.Instance.race == StatusRace.PreparingGame) StatusStartRace();        
    }

    //get fuel in the correct way
    private void FuelControl()
    {
        fuelBar.value = sliderBarValue;        
    }

    public void StatusTurboCar()
    {        
        if (qntTurbo == 3) return;
        imgTurbo[qntTurbo].enabled = false;
        qntTurbo += 1;        
    }

    public void StatusStartRace()
    {
        TimerStartRace.text = GameManager.Instance.timerRace.ToString();
    }
}
