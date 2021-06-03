﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatusCar
{
    Drive,
    PitStop,
    Broken,
    FinishedRace
}

public class PlayerCar : MonoBehaviour
{
    public static PlayerCar instance;

    WheelManager[] wheelGuide;

    public StatusCar statusPlayer;

    public float aceleration = 0f;
    public Vector3 forceFinal;

    public WheelCollider[] wheelsCar;
    private float driveCar = 0f;    

    private Rigidbody rb;    

    private float veloKMH, rpm;
    public float instabilityHang;

    public float[] raceChenges;
    private int changeCurrent = 0;

    public float maxRPM;
    public float minRPM;

    public float forceStop;
    public float maxTorque;

    public float somPitch;

    public Transform massCenter;
    
    private int laps;

    public float fuelCar;

    public AudioClip somCar;
    public AudioClip somKid;
    public AudioClip somKidGrass;

    public AudioSource audioCar;
    public AudioSource audioSkid;

    public AnimationCurve curveWheel;

    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.centerOfMass = massCenter.position;
        audioCar.clip = somCar;

        wheelGuide = new WheelManager[wheelsCar.Length];
        fuelCar = 100;

        for(int i = 0; i < wheelsCar.Length; i++)
        {
            wheelGuide[i] = wheelsCar[i].GetComponent<WheelManager>();
        }
    }

    void Update()
    {
        driveCar = Input.GetAxis("Horizontal");
        aceleration = Input.GetAxis("Vertical");
    }

    public void AddLaps()
    {
        laps++;
        Debug.Log("Volta atual: " + laps);
    }

    private void FixedUpdate()
    {
        //guiar o carro
        for (int i = 0; i < wheelsCar.Length; i++)
        {
            wheelsCar[i].steerAngle = driveCar * curveWheel.Evaluate(veloKMH);
            wheelsCar[i].motorTorque = 1f;

            //carro sai da estrada
            if (wheelGuide[i].wheelCurrent != 0)
            {                
                rb.AddTorque((transform.up * (instabilityHang/2f) * veloKMH / 45f) * driveCar);                
                /*if(audioSkid.clip != somKidGrass)
                {
                    audioSkid.clip = somKidGrass;
                    audioSkid.Play();
                }*/
            }
            else
            {
                if (audioSkid.clip != somKid)
                {
                    audioSkid.clip = somKid;
                    audioSkid.Play();
                }
            }

        }

        //velocidade em RPM
        veloKMH = rb.velocity.magnitude * 3.6f;
        rpm = veloKMH * raceChenges[changeCurrent] * 15f;        

        if (rpm > maxRPM)
        {
            changeCurrent++;
            if (changeCurrent == raceChenges.Length)
            {
                changeCurrent--;
            }
        }
        if (rpm < minRPM)
        {
            changeCurrent--;
            if (changeCurrent < 0)
            {
                changeCurrent = 0;
            }
        }

        //Força
        if (aceleration < -0.2f)
        {
            rb.AddForce(-transform.forward * forceStop);
            rb.AddTorque((transform.up * instabilityHang * veloKMH / 60f) * driveCar);
            aceleration = 0;
        }

        forceFinal = transform.forward * (maxTorque / (changeCurrent + 1) + maxTorque / 1.25f) * aceleration;
        rb.AddForce(forceFinal);

        //add som na aceleração do carro
        audioCar.pitch = rpm / somPitch;

        //som da derrapagem
        if (veloKMH >= 20f)
        {
            float angulo = Vector3.Angle(transform.forward, rb.velocity);
            float valorFinal = (angulo / 10f) - 0.3f;
            audioSkid.volume = Mathf.Clamp(valorFinal, 0f, 1f);
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 128, 32), rpm + "RPM");
        GUI.Label(new Rect(20, 40, 128, 32), (changeCurrent + 1).ToString());
        GUI.Label(new Rect(20, 60, 128, 32), veloKMH + "KMH");
        GUI.Label(new Rect(20, 80, 128, 32), forceFinal.magnitude.ToString());

        GUI.Label(new Rect(20, 120, 128, 32), "Fuel: " + fuelCar);
        GUI.Label(new Rect(20, 100, 128, 32), "Timer: " + GameManager.instance.returnTime().ToString()); 
       
    }


}
