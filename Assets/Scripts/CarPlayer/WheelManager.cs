using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelManager : MonoBehaviour
{
    public WheelCollider[] typeWheels;

    WheelCollider wc;

    RaycastHit hit;

    public int wheelCurrent = 0;

    private void Start()
    {
        wc = GetComponent<WheelCollider>();
    }

    private void FixedUpdate()
    {
        if(Physics.Raycast(transform.position, -transform.up, out hit, 2f))
        {
            if(hit.collider.tag == "Roads")
            {
                if(wheelCurrent != 0)
                {
                    wheelCurrent = 0;
                    wc.sidewaysFriction = typeWheels[0].sidewaysFriction;
                }
            }
            if (hit.collider.tag == "Grass")
            {
                if (wheelCurrent != 1)
                {
                    wheelCurrent = 1;
                    wc.sidewaysFriction = typeWheels[1].sidewaysFriction;
                }
            }
        }
    }
}
