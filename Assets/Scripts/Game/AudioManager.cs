using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if(SceneManager.GetActiveScene().name.Contains("Track"))
        {
            if (audioM.isPlaying)
            {                
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
            if (audioS.isPlaying)
            {
                audioS.Stop();
            }
            if (!audioM.isPlaying)
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
