using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCar : MonoBehaviour
{
    private CarManager c;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.CompareTag("Player"))
        {            
            //c = other.transform.root.GetComponent<CarManager>();
            //Checkpoint.Instance.ReturnPosCar(c,c.completedLaps);
        }
    }
}
