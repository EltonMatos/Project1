using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListTrack : MonoBehaviour
{    
    public Sprite[] imagemSp;
    private int aux = 0;

    public Image trackCurrent;

    public int idTrack;
    

    private void Start()
    {
        idTrack = aux + 1;        
        trackCurrent = gameObject.GetComponentInChildren<Image>();
        trackCurrent.sprite = imagemSp[0];        
    }    

    public void NextTrack()
    {
        if (aux < imagemSp.Length -1)
        {
            aux++;

            //trackCurrent.sprite = imagemSp[PlayerPrefs.GetInt("Track" + aux)];
            trackCurrent.sprite = imagemSp[aux];            
        }
    }

    public void PreviusTrack()
    {
        if (aux > 0)
        {
            aux--;
            trackCurrent.sprite = imagemSp[aux];            
        }
    }
}
