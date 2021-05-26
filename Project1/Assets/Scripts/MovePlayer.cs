using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{    
    private float turnSpeed = 90f;
    public float aceleration = 0f;
    public float force = 0f;


    public WheelCollider[] wheelsCar;
    private float buttonGui = 0f;

    public AudioClip somCar;
    public AudioSource audioCar;

    private Rigidbody rb;
    private float veloKMH;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioCar.clip = somCar;
    }

    void Update()
    {
        buttonGui = Input.GetAxis("Horizontal");
        aceleration = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < wheelsCar.Length; i++)
        {
            wheelsCar[i].steerAngle = buttonGui * 15f;
            wheelsCar[i].motorTorque = 1f;
        }

        rb.AddForce(transform.forward * force * aceleration);

        //add som na aceleração do carro
        veloKMH = rb.velocity.magnitude * 3.6f;
        audioCar.pitch = 0.6f + veloKMH / 60f;
    }

       
}
