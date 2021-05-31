using System.Collections;
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

        foreach(Checkpoint ch in checkpoints)
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
        foreach(Checkpoint check in checkpoints)
        {
            check.car = null;
        }
    }
}
