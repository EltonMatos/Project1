using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject car = null;    

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponent<PlayerCar>())
        {
            car = other.transform.root.gameObject;
            //TODO removing this to have it implemented correctly
            //PlayerCar.Instance.fuelCar -= 10;            
        }
    }

    public bool ReadyCar()
    {
        if (car != null)
        {
            return true;
        }
        return false;
    }
}
