using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_Menu : MonoBehaviour
{
    public Slider lapsBar;
    public float sliderBarValue;

    public Text numLaps;

    private void Start()
    {
        lapsBar.minValue = 1f;        
        lapsBar.value = lapsBar.minValue;        
    }

    private void Update()
    {
        LapsControl();

    }

    private void LapsControl()
    {
        sliderBarValue = Mathf.RoundToInt(lapsBar.value);
        numLaps.text = sliderBarValue.ToString();
        GameManager.Instance.lapsMax = sliderBarValue;  
        
    }

    public void StartGame()
    {        
        GameManager.Instance.race = StatusRace.PreparingGame;
    }
}
