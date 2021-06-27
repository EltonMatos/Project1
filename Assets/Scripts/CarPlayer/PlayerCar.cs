using System.Collections;
using System.Collections.Generic;
using CarPlayer;
using Network;
using Photon.Pun;
using UnityEngine;


public enum StatusCar
{
    Stop,
    Drive,
    PitStop,
    Broken,
    FinishedRace,
    LockedCar,
}

public class PlayerCar : MonoBehaviour
{
    public PhotonView photonView;

    private CarManager car;

    WheelManager[] wheelGuide;

    public StatusCar statusPlayer;

    public int idCar;

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

    public float fuelCar;
    public float damagedCar;

    ParticleSystem smokenParticicle;
    ParticleSystem.EmissionModule emissionModule;

    public float turbo;

    public AudioClip somCar;
    public AudioClip somKid;
    public AudioClip somKidGrass;

    public AudioSource audioCar;
    public AudioSource audioSkid;

    public AnimationCurve curveWheel;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        car = GetComponent<CarManager>();
        smokenParticicle = GetComponentInChildren<ParticleSystem>();
        emissionModule = smokenParticicle.emission;
        emissionModule.enabled = false;
        
        audioCar.clip = somCar;

        wheelGuide = new WheelManager[wheelsCar.Length];
        fuelCar = 100;
        damagedCar = 0;
        turbo = 3;

        for (int i = 0; i < wheelsCar.Length; i++)
        {
            wheelGuide[i] = wheelsCar[i].GetComponent<WheelManager>();
        }


        SetupNetworkBasedValues();
    }

    private void SetupNetworkBasedValues()
    {
        //setup id
        int playerRoomId = GameRoom.Instance.GetId(photonView.Owner);
        if (playerRoomId < 999)
        {
            idCar = playerRoomId;
        }

        //setup color
        CarColors color = GameRoom.Instance.GetColor(photonView.Owner);
        var mesh = CarColorManager.Instance.GetMesh(color);
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            driveCar = Input.GetAxis("Horizontal");
            acceleration = Input.GetAxis("Vertical");

            if (turbo > 0 && statusPlayer != StatusCar.Broken)
            {
                UiManager.Instance.StatusTurboCar();
            }

            UpdateStatusCar();

            if (statusPlayer != StatusCar.LockedCar)
            {
                StatusFuelCar();
            }

            if (statusPlayer == StatusCar.FinishedRace) audioCar.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Space) && turbo > 0 && statusPlayer != StatusCar.Broken)
        {
            turbo--;
            maxTorque = 20000;
            StartCoroutine(TurboCar());            
        }

        if (statusPlayer != StatusCar.LockedCar)
        {
            StatusDamagedCar();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.race == StatusRace.StartRace)
        {
            if (statusPlayer == StatusCar.Drive || statusPlayer == StatusCar.Stop) DriveCar();
            if (statusPlayer == StatusCar.PitStop || statusPlayer == StatusCar.Broken) SlowDownCar();
            if (statusPlayer == StatusCar.LockedCar || statusPlayer == StatusCar.FinishedRace) StopCar();

            if (veloKMH <= 1 && statusPlayer != StatusCar.Broken && statusPlayer != StatusCar.LockedCar)
            {
                statusPlayer = StatusCar.Stop;
            }
        }
    }

    private void moveCar()
    {
        //guiar o carro        
        for (int i = 0; i < wheelsCar.Length; i++)
        {
            wheelsCar[i].steerAngle = driveCar * curveWheel.Evaluate(veloKMH);
            wheelsCar[i].motorTorque = 1f;

            //carro sai da estrada
            if (wheelGuide[i].wheelCurrent != 0)
            {
                rb.AddTorque((transform.up * (instabilityHang / 2f) * veloKMH / 45f) * driveCar);
                if (audioSkid.clip != somKidGrass)
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
            }
        }

        //velocidade em RPM
        veloKMH = rb.velocity.magnitude * 2.5f;
        rpm = veloKMH * raceChenges[changeCurrent] * 15f;

        //Força
        if (acceleration < -0.8f)
        {
            rb.AddForce(-transform.forward * forceStop);
            rb.AddTorque((transform.up * instabilityHang * veloKMH / 60f) * driveCar);
            acceleration = 0;
        }

        if (veloKMH <= 120f)
        {
            forceFinal = transform.forward * (maxTorque / (changeCurrent + 1) + maxTorque / 1f) * (acceleration * 1.2f);
            rb.AddForce(forceFinal);
        }
    }

    private void DriveCar()
    {
        moveCar();

        statusPlayer = StatusCar.Drive;

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
        if (veloKMH > 40f)
        {
            forceFinal = -transform.forward * (10000 / (changeCurrent + 1) + 10000 / 1f) * acceleration;
            rb.AddForce(forceFinal);
        }

        if (statusPlayer != StatusCar.LockedCar)
        {
            moveCar();           

            audioCar.pitch = rpm / somPitch;
        }
    }

    IEnumerator TurboCar()
    {
        yield return new WaitForSeconds(1);
        maxTorque = 7000;
    }

    private void StopCar()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        for (int i = 0; i < wheelsCar.Length; i++)
        {
            wheelsCar[i].steerAngle = 0f;
            wheelsCar[i].motorTorque = 0f;
        }

        if (statusPlayer == StatusCar.LockedCar) StartCoroutine(TimePitStop());
        audioCar.volume = 0;
    }

    private void UpdateStatusCar()
    {
        UiManager.Instance.sliderBarValue = fuelCar;
        UiManager.Instance.statusCar.text = statusPlayer.ToString();
    }

    private void StatusDamagedCar()
    {
        if (damagedCar >= 50 && statusPlayer != StatusCar.PitStop)
        {
            emissionModule.enabled = true;
            statusPlayer = StatusCar.Broken;
        }
    }

    private void StatusFuelCar()
    {
        if (fuelCar <= 0 && statusPlayer != StatusCar.PitStop)
        {
            statusPlayer = StatusCar.Broken;
        }
    }

    IEnumerator TimePitStop()
    {
        audioCar.volume = 0;
        if (fuelCar == 100 && damagedCar <= 0 && statusPlayer != StatusCar.FinishedRace) statusPlayer = StatusCar.Drive;
        yield return new WaitForSeconds(3);
        if (statusPlayer == StatusCar.LockedCar)
        {
            if (fuelCar < 100)
            {
                fuelCar += 5;
            }

            if (damagedCar > 0)
            {
                damagedCar -= 5;
            }

            if (turbo < 3)
            {
                turbo = 3;
            }
            emissionModule.enabled = false;
        }

        audioCar.volume = 1;
    }

    private void PitStopCar(PitStop pitStop)
    {
        if (idCar == pitStop.idPitStop && statusPlayer == StatusCar.PitStop)
        {
            var pitStopGameObject = pitStop.gameObject;
            transform.position = pitStopGameObject.transform.position;
            transform.rotation = pitStopGameObject.transform.rotation;
            statusPlayer = StatusCar.LockedCar;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PitStopEntrance"))
        {
            statusPlayer = StatusCar.PitStop;
        }

        if (other.gameObject.CompareTag("PitStop"))
        {
            PitStop pitStop = other.GetComponentInChildren<PitStop>();
            PitStopCar(pitStop);
        }

        if (other.gameObject.CompareTag("Checkpoint"))
        {
            if (fuelCar >= 0) fuelCar -= 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PitStopExit"))
        {
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
            damagedCar += 2;
        }
    }

    private void OnGUI()
    {
        if (photonView.IsMine)
        {
            GUI.Label(new Rect(20, 20, 128, 32), rpm + "RPM");
            GUI.Label(new Rect(20, 40, 128, 32), (changeCurrent + 1).ToString());
            GUI.Label(new Rect(20, 60, 128, 32), veloKMH + "KMH");

            GUI.Label(new Rect(20, 80, 128, 32), "Damaged: " + damagedCar);            
            GUI.Label(new Rect(20, 100, 128, 32), "Timer: " + car.ReturnTime());
        }
    }
}