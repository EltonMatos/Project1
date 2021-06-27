using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentScene : MonoBehaviour
{
    public int phase = -1;

    public static CurrentScene instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += CheckPhase;
    }

    void CheckPhase(Scene cena, LoadSceneMode modo)
    {
        phase = SceneManager.GetActiveScene().buildIndex;
    }
}
