using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListTrack : MonoBehaviour
{
    public List<int> tracks;
    public Sprite[] imagemSp;
    private int aux = 0;

    public Image trackCurrent;

    public int idTrack;

    private void Awake()
    {
        tracks = new List<int>();
        tracks.Add(0);

        if (!PlayerPrefs.HasKey("Track1"))
        {
            PlayerPrefs.SetInt("Track1", tracks[0]);
            PlayerPrefs.SetInt("list_Count", 1);//novo
            print("salvo");

        }

        for (int i = 1; i < PlayerPrefs.GetInt("list_Count"); i++)
        {
            tracks.Add(PlayerPrefs.GetInt("Track" + i));
        }
    }

    private void Start()
    {
        idTrack = 0;
        
        trackCurrent = gameObject.GetComponent<Image>();
        trackCurrent.sprite = imagemSp[PlayerPrefs.GetInt("Track1" + tracks[0])];
        Debug.Log("Track " + tracks.Count + " sprite " + imagemSp.Length);
    }    

    public void NextTrack()
    {
        if (aux < imagemSp.Length -1)
        {
            aux++;
            trackCurrent.sprite = imagemSp[PlayerPrefs.GetInt("Track" + aux)];
            print("next");
        }
    }

    public void PreviusTrack()
    {
        if (aux >= 0)
        {
            aux--;
            trackCurrent.sprite = imagemSp[PlayerPrefs.GetInt("Track " + aux)];
            print("previus");
        }
    }
}
