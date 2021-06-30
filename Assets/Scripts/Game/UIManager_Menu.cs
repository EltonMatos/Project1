using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_Menu : MonoBehaviour
{
    public PhotonView PhotonView;
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
        PhotonView.RPC("UpdateNumLaps", RpcTarget.OthersBuffered, sliderBarValue);
    }

    [PunRPC]
    public void UpdateNumLaps(int num)
    {
        GameManager.Instance.lapsMax = num;
    }
}
