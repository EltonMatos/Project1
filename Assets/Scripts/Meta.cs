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
        CarManager car = other.transform.root.GetComponent<CarManager>();
        car.checkLap = true;
        
        foreach (Checkpoint ch in checkpoints)
        {
            if (!ch.CheckedCar(car, car.completedLaps))
            {
                //Checkpoint.Instance.RegisterCar(car, car.completedLaps);
                //car.positionCar = Checkpoint.Instance.ReturnPosCar(car, car.completedLaps);
                Debug.Log("Volta invalidade");
                ResetCheckpoints(car);                
                return;
            }
        }
        car.completedLaps++;        
        ResetCheckpoints(car);
        
    }

    void ResetCheckpoints(CarManager car)
    {
        car.timer = 0;        
        /*foreach (Checkpoint check in checkpoints)
        {
            check.car = null;
        }*/
    }
}
