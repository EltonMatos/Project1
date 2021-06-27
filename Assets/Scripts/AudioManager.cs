using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip audioMenu;
    public AudioSource audioM;
    public AudioSource audioS;
    public AudioClip[] clip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioS = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(CurrentScene.instance.phase != 0 && CurrentScene.instance.phase != 1)
        {
            if (audioM.isPlaying)
            {
                print("entrou");
                audioM.Stop();
            }
            if (!audioS.isPlaying)
            {                
                audioS.clip = GetRandom();
                audioS.Play();
            }                       
        }
        else
        {
            if (!audioS.isPlaying)
            {
                audioM.clip = audioMenu;
                audioM.Play();
            }
        }
    }

    AudioClip GetRandom()
    {
        return clip[Random.Range(0, clip.Length)];
    }
}
