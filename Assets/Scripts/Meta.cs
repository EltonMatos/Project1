﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : MonoBehaviour
{
    public Checkpoint[] checkpoints;    

    private void Awake()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCar x = other.transform.root.GetComponent<PlayerCar>();
        GameManager.Instance.checkLap = true;
        GameManager.Instance.checkOldLap = true;
        GameManager.Instance.completedLaps++;
        foreach (Checkpoint ch in checkpoints)
        {
            if (!ch.ReadyCar())
            {
                Debug.Log("Volta invalidade");
                ResetCheckpoints();                
                return;
            }
        }
        x.AddLaps();        
        ResetCheckpoints();        
    }

    void ResetCheckpoints()
    {
        GameManager.Instance.timer = 0;
        foreach (Checkpoint check in checkpoints)
        {
            check.car = null;
        }
    }
}
