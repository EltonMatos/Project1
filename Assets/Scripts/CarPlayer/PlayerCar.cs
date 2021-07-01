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
    public WheelCollider[] frontWheelsCar;

    private Rigidbody rb;

    private float veloKMH, rpm;
    public float instabilityHang;

    public float[] raceChanges;
    private int changeCurrent = 0;

    public float maxRPM;
    public float minRPM;

    public float forceStop;
    public float maxTorque;

    public float somPitch;

    public float fuelCar;
    public float damagedCar;


    public ParticleSystem smokenParticicle;
    ParticleSystem.EmissionModule emissionModuleSmoke;

    public ParticleSystem fireParticicle;
    ParticleSystem.EmissionModule emissionModuleFire;

    public float turbo;

    public AudioClip somCar;
    public AudioClip somKid;

    public AudioSource audioCar;
    public AudioSource audioSkid;

    private int vol = 0;

    public AnimationCurve curveWheel;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        car = GetComponent<CarManager>();

        emissionModuleSmoke = smokenParticicle.emission;
        emissionModuleSmoke.enabled = false;

        emissionModuleFire = fireParticicle.emission;
        emissionModuleFire.enabled = false;

        audioCar.clip = somCar;

        wheelGuide = new WheelManager[frontWheelsCar.Length];
        fuelCar = 100;
        damagedCar = 0;
        turbo = 3;

        for (int i = 0; i < frontWheelsCar.Length; i++)
        {
            wheelGuide[i] = frontWheelsCar[i].GetComponent<WheelManager>();
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

            if (Input.GetKeyDown(KeyCode.Space) && turbo > 0 && statusPlayer != StatusCar.Broken)
            {
                emissionModuleFire.enabled = true;
                photonView.RPC("ToggleBoostForCar", RpcTarget.Others, photonView.Owner.ActorNumber, true);

                turbo--;
                maxTorque = 20000;
                StartCoroutine(TurboCar());
                UiManager.Instance.StatusTurboCar();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                vol++;
                audioCar.volume = vol;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                vol--;
                audioCar.volume = vol;
            }

            UpdateStatusCar();

            if (statusPlayer != StatusCar.LockedCar)
            {
                StatusFuelCar();
            }

            if (statusPlayer == StatusCar.FinishedRace) audioCar.Stop();
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

    private void MoveCar()
    {
        //guiar o carro        
        for (int i = 0; i < frontWheelsCar.Length; i++)
        {
            //TODO add call to steer visually the wheel
            frontWheelsCar[i].steerAngle = driveCar * curveWheel.Evaluate(veloKMH);
            frontWheelsCar[i].motorTorque = 1f;

            //carro sai da estrada
            if (wheelGuide[i].wheelCurrent != 0)
            {
                rb.AddTorque((transform.up * (instabilityHang / 2f) * veloKMH / 45f) * driveCar);
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
        rpm = veloKMH * raceChanges[changeCurrent] * 15f;

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
        MoveCar();

        statusPlayer = StatusCar.Drive;

        if (rpm > maxRPM)
        {
            changeCurrent++;
            if (changeCurrent == raceChanges.Length)
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
            MoveCar();
            audioCar.pitch = rpm / somPitch;
        }
    }    

    private void StopCar()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        for (int i = 0; i < frontWheelsCar.Length; i++)
        {
            frontWheelsCar[i].steerAngle = 0f;
            frontWheelsCar[i].motorTorque = 0f;
        }

        if (statusPlayer == StatusCar.LockedCar) StartCoroutine(TimePitStop());
        audioCar.volume = 0;
    }

    private void UpdateStatusCar()
    {
        UiManager.Instance.sliderBarValue = fuelCar;
        UiManager.Instance.statusCar.text = statusPlayer.ToString();
        UiManager.Instance.numLaps.text = car.completedLaps + " / " + GameManager.Instance.lapsMax;
    }

    private void StatusDamagedCar()
    {
        if (photonView.IsMine && damagedCar >= 40 && statusPlayer != StatusCar.PitStop)
        {
            emissionModuleSmoke.enabled = true;
            statusPlayer = StatusCar.Broken;
            photonView.RPC("ToggleSmokeForCar", RpcTarget.Others, photonView.Owner.ActorNumber, true);
        }
    }

    private void StatusFuelCar()
    {
        if (fuelCar <= 0 && statusPlayer != StatusCar.PitStop)
        {
            statusPlayer = StatusCar.Broken;
        }
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

    /*private void OnGUI()
    {
        if (photonView.IsMine)
        {
            GUI.Label(new Rect(20, 20, 128, 32), rpm + "RPM");
            GUI.Label(new Rect(20, 40, 128, 32), (changeCurrent + 1).ToString());
            GUI.Label(new Rect(20, 60, 128, 32), veloKMH + "KMH");

            GUI.Label(new Rect(20, 80, 128, 32), "Damaged: " + damagedCar);            
            GUI.Label(new Rect(20, 100, 128, 32), "Timer: " + car.ReturnTime());
        }
    }*/

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

            emissionModuleSmoke.enabled = false;
            photonView.RPC("ToggleSmokeForCar", RpcTarget.Others, photonView.Owner.ActorNumber, false);
        }

        audioCar.volume = 1;
    }

    IEnumerator TurboCar()
    {
        yield return new WaitForSeconds(1);
        maxTorque = 7000;
        emissionModuleFire.enabled = false;
        photonView.RPC("ToggleBoostForCar", RpcTarget.Others, photonView.Owner.ActorNumber, false);
    }

    [PunRPC]
    public void ToggleSmokeForCar(int actorNumber, bool value)
    {
        if (photonView.Owner.ActorNumber == actorNumber)
        {
            emissionModuleSmoke.enabled = value;
        }
    } 
    
    [PunRPC]
    public void ToggleBoostForCar(int actorNumber, bool value)
    {
        if (photonView.Owner.ActorNumber == actorNumber)
        {
            emissionModuleFire.enabled = value;
        }
    }
}