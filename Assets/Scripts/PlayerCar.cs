using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public enum StatusCar
{
    Stop,
    Drive,
    PitStop,
    Broken,
    FinishedRace,    
}

public class PlayerCar : MonoBehaviour
{
    public PhotonView photonView;
    
    WheelManager[] wheelGuide;

    public StatusCar statusPlayer;

    private float acceleration = 0f;
    private float driveCar = 0f;

    public Vector3 forceFinal;
    public WheelCollider[] wheelsCar;        

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
    
    private int laps;
    private float[] TimeLaps;
    //private int posLabel = 20;

    public float fuelCar;
    public float damagedCar;

    public AudioClip somCar;
    public AudioClip somKid;
    public AudioClip somKidGrass;

    public AudioSource audioCar;
    public AudioSource audioSkid;

    public AnimationCurve curveWheel;

    private bool pitStop;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.centerOfMass = massCenter.position;
        audioCar.clip = somCar;
        pitStop = false;

        wheelGuide = new WheelManager[wheelsCar.Length];
        fuelCar = 100;
        damagedCar = 0;

        for(int i = 0; i < wheelsCar.Length; i++)
        {
            wheelGuide[i] = wheelsCar[i].GetComponent<WheelManager>();
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            driveCar = Input.GetAxis("Horizontal");
            acceleration = Input.GetAxis("Vertical");
        }

        UpdateStatusCar();
        StatusDamagedCar();        
    }

    public void AddLaps()
    {        
        /*for (int i = 0; i < GameManager.Instance.lapsMax; i++)
        {
            GUI.Label(new Rect(40, posLabel, 128, 32), laps + ": " +  "Timer: " + GameManager.Instance.ReturnTime().ToString());
            posLabel += 20;
        }*/

        laps++;
        Debug.Log("Volta atual: " + laps);
    }

    private void FixedUpdate()
    {
        if(statusPlayer != StatusCar.PitStop) DriveCar();
        if(statusPlayer == StatusCar.PitStop || statusPlayer == StatusCar.Broken) SlowDownCar();

        if (veloKMH > 1 && pitStop == false)
        {
            statusPlayer = StatusCar.Drive;
        }
        if (veloKMH <= 1 && statusPlayer != StatusCar.Broken)
        {
            statusPlayer = StatusCar.Stop;
        }

    }

    private void DriveCar()
    {
        //guiar o carro        
        for (int i = 0; i < wheelsCar.Length; i++)
        {
            wheelsCar[i].steerAngle = driveCar * curveWheel.Evaluate(veloKMH);
            wheelsCar[i].motorTorque = 1f;

            //carro sai da estrada
            /*if (wheelGuide[i].wheelCurrent != 0)
            {
                rb.AddTorque((transform.up * (instabilityHang / 2f) * veloKMH / 45f) * driveCar);
                if(audioSkid.clip != somKidGrass)
                {
                    audioSkid.clip = somKidGrass;
                    audioSkid.Play();
                }
            }
            else
            {
                if (audioSkid.clip != somKid)
                {
                    audioSkid.clip = somKid;
                    audioSkid.Play();
                }
            }*/
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
        if (acceleration < -0.2f)
        {
            rb.AddForce(-transform.forward * forceStop);
            rb.AddTorque((transform.up * instabilityHang * veloKMH / 60f) * driveCar);
            acceleration = 0;
        }

        forceFinal = transform.forward * (maxTorque / (changeCurrent + 1) + maxTorque / 1.25f) * acceleration;
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

    private void SlowDownCar()
    {
        for (int i = 0; i < wheelsCar.Length; i++)
        {
            wheelsCar[i].steerAngle = driveCar * curveWheel.Evaluate(veloKMH);
            wheelsCar[i].motorTorque = 1f;
        }
        veloKMH = 30f;
        rpm = 2000;

        if (acceleration < -0.2f)
        {
            rb.AddForce(-transform.forward * forceStop);
            rb.AddTorque((transform.up * instabilityHang * veloKMH / 60f) * driveCar);
            acceleration = 0;
        }

        forceFinal = transform.forward * (2000 / (changeCurrent + 1) + 2000 / 1.25f) * acceleration;
        rb.AddForce(forceFinal);

        audioCar.pitch = rpm / somPitch;
    }

    private void UpdateStatusCar()
    {
        UiManager.Instance.sliderBarValue = fuelCar;
        UiManager.Instance.statusCar.text = statusPlayer.ToString();
    }

    private void StatusDamagedCar()
    {
        if(damagedCar >= 30)
        {

        }
        if(damagedCar >= 40)
        {

        }
        if(damagedCar >= 60)
        {
            statusPlayer = StatusCar.Broken;
        }
    }

    private void PitStopCar()
    {
        //rb.AddForce(-transform.forward * forceStop);
        if (statusPlayer == StatusCar.PitStop && statusPlayer == StatusCar.Stop)
        {
            fuelCar = 100;
            damagedCar = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PitStopEntrance"))
        {
            pitStop = true;
            statusPlayer = StatusCar.PitStop;
        }        
        if (other.gameObject.CompareTag("PitStop"))
        {
            PitStopCar();
            statusPlayer = StatusCar.Stop;
        }
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            fuelCar -= 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PitStopExit"))
        {
            pitStop = false;
            statusPlayer = StatusCar.Drive;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            damagedCar += 5;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            damagedCar += 3;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 128, 32), rpm + "RPM");
        GUI.Label(new Rect(20, 40, 128, 32), (changeCurrent + 1).ToString());
        GUI.Label(new Rect(20, 60, 128, 32), veloKMH + "KMH");
        GUI.Label(new Rect(20, 80, 128, 32), forceFinal.magnitude.ToString());

        GUI.Label(new Rect(20, 100, 128, 32), "Fuel: " + fuelCar);
        GUI.Label(new Rect(20, 120, 128, 32), "Damaged: " + damagedCar);
        GUI.Label(new Rect(20, 140, 128, 32), "StatusCar: " + statusPlayer);
        GUI.Label(new Rect(20, 160, 128, 32), "Timer: " + GameManager.Instance.ReturnTime().ToString()); 
       
    }



}
