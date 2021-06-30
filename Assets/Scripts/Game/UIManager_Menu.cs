using System.Collections;
using System.Collections.Generic;
using Network;
using Photon.Pun;
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

    public void ValueChanged()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            sliderBarValue = Mathf.RoundToInt(lapsBar.value);
            numLaps.text = sliderBarValue.ToString();
            GameManager.Instance.lapsMax = sliderBarValue;
            GameRoom.Instance.UpdateNumberOfLaps(sliderBarValue);
        }
    }
    
}