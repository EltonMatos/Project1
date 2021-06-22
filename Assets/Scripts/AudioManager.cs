using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip audioMenu;
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
        if (!audioS.isPlaying)
        {            
            audioS.clip = GetRandom();
            audioS.Play();
        }
    }

    AudioClip GetRandom()
    {
        return clip[Random.Range(0, clip.Length)];
    }
}
