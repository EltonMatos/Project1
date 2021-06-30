using UnityEngine;

public class Meta : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {        
        CarManager car = other.transform.root.GetComponent<CarManager>();

        car.AddLaps();
    }
}
