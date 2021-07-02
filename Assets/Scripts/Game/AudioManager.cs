using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip audioMenu;
    public AudioSource audioM;
    public AudioSource audioS;
    public AudioClip[] clip;

    private float volumeCar, volumeMusic;

    private float vol = 0.1f;

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

    private void Start()
    {
        audioS.volume = 0.2f;
        audioM.volume = 0.2f;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {            
            vol = vol - 0.1f;
            if (vol < 0) vol = 0;
            audioM.volume = vol;
         
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {            
            vol = vol + 0.1f;
            if (vol > 1) vol = 1;
            audioM.volume = vol;            
        }

        if (SceneManager.GetActiveScene().name.Contains("Track"))
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

    public void VolumeSoundCar(float volume)
    {
        volumeCar = volume;
        GameObject[] vol = GameObject.FindGameObjectsWithTag("Car");
        for(int i = 0; i < vol.Length; i++)
        {
            vol[i].GetComponent<AudioSource>().volume = volumeCar;
        }
    }

    public void VolumeMusic(float volume)
    {
        volumeMusic = volume;
        GameObject[] vol = GameObject.FindGameObjectsWithTag("Music");
        for (int i = 0; i < vol.Length; i++)
        {
            vol[i].GetComponent<AudioSource>().volume = volumeMusic;
        }
    }

    public void VolumeMaster(float volume)
    {        
        AudioListener.volume = volume;
    }
}
