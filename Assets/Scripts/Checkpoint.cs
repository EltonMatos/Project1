using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{    
    //TODO not in use, is it needed?
    public static Checkpoint Instance;

    public List<Laps> lapRace;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        lapRace = new List<Laps>();
        for(int i = 0; i < GameManager.Instance.lapsMax; i++)
        {
            lapRace.Add(new Laps());
        }
    }

    private void OnTriggerEnter(Collider other)
    {        
        CarManager c = other.transform.root.GetComponent<CarManager>();

        RegisterCar(c, c.completedLaps);
        c.positionCar = ReturnPosCar(c, c.completedLaps);
    }

    public bool CheckedCar(CarManager car, int vol)
    {
        for (int i = 0; i < lapRace[vol].car.Count; i++)
        {
            if (lapRace[vol].car[i] == car)
            {
                return true;
            }
        }

        return false;
    }

    private void RegisterCar(CarManager car, int vol)
    {        
        lapRace[vol].car.Add(car);
    }

    public int ReturnPosCar(CarManager car, int vol)
    {
        for(int i = 0; i < lapRace[vol].car.Count; i++)
        {
            if(lapRace[vol].car[i] == car)
            {
                return i + 1;
            }
        }

        return -1;
    }

    /*public bool ReadyCar()
    {
        if (car != null)
        {
            return true;
        }
        return false;
    }*/    

    public class Laps
    {
        public List<CarManager> car;

        public Laps()
        {
            car = new List<CarManager>();
        }
    }
}
