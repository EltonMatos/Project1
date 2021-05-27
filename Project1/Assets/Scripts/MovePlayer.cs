using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{  
    public float aceleration = 0f;
    public Vector3 forceFinal;
    public float forceStop;
    public float maxTorque;


    public WheelCollider[] wheelsCar;
    private float buttonGui = 0f;

    public AudioClip somCar;
    public AudioSource audioCar;

    private Rigidbody rb;

    public AnimationCurve curveWheel;

    private float veloKMH, rpm;

    public float[] raceChenges;
    private int changeCurrent = 0;

    public float maxRPM;
    public float minRPM;

    public float somPitch;

    public Transform massCenter;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.centerOfMass = massCenter.position;
        audioCar.clip = somCar;
    }

    void Update()
    {
        buttonGui = Input.GetAxis("Horizontal");
        aceleration = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        //guiar o carro
        for (int i = 0; i < wheelsCar.Length; i++)
        {
            wheelsCar[i].steerAngle = buttonGui * curveWheel.Evaluate(veloKMH);
            wheelsCar[i].motorTorque = 1f;
        }

        //velocidade em RPM
        veloKMH = rb.velocity.magnitude * 3.6f;
        rpm = veloKMH * raceChenges[changeCurrent] * 15f;

        if(rpm > maxRPM)
        {
            changeCurrent++;
            if(changeCurrent == raceChenges.Length)
            {
                changeCurrent--;
            }
        }
        if(rpm < minRPM)
        {
            changeCurrent--;
            if (changeCurrent < 0)
            {
                changeCurrent = 0;
            }
        }

        //Força
        if(aceleration < -0.5f)
        {
            rb.AddForce(-transform.forward * forceStop);
            aceleration = 0;
        }

        forceFinal = transform.forward * (maxTorque / (changeCurrent + 1) + maxTorque/1.85f) * aceleration;
        rb.AddForce(forceFinal);

        //add som na aceleração do carro
        audioCar.pitch = rpm / somPitch;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 128, 32), rpm + "RPM");
        GUI.Label(new Rect(20, 40, 128, 32), (changeCurrent+1).ToString());
        GUI.Label(new Rect(20, 60, 128, 32), veloKMH + "KMH");
        GUI.Label(new Rect(20, 80, 128, 32), forceFinal.magnitude.ToString());
    }


}
