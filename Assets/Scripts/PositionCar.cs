using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCar : MonoBehaviour
{
    private CarManager c;

    public List<Laps> lapRace;

    public int idPosCurrent;

    private Transform checkPosition;
    private Transform checkPositionPrevius;
    private Transform positionCar;

    public GameObject posStart;

    private float _distance;

    private PositionCar car;

    private void Start()
    {
        car = GetComponentInParent<PositionCar>();
        positionCar = GetComponent<Transform>();
        checkPositionPrevius = GetComponent<Transform>();
        checkPositionPrevius.position = posStart.transform.position;               

        lapRace = new List<Laps>();
        
        for (int i = 0; i < GameManager.Instance.lapsMax; i++)
        {
            lapRace.Add(new Laps());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CarManager c = other.transform.root.GetComponent<CarManager>();

        // wrong way
        positionCar.position = c.transform.position;
        _distance = Vector3.Distance(positionCar.position, checkPositionPrevius.position);
        c.wrongWay = false;
        checkPositionPrevius.position = positionCar.position;

        //register position
        //RegisterCar(c, c.completedLaps);
        //c.positionCar = ReturnPosCar(c, c.completedLaps);


        /*if (c.idCarPosCurrent < c.idCarPosPrevius && idPosCurrent != 1)
        {
            c.wrongWay = true;
            c.idCarPosPrevius = c.idCarPosCurrent;
        }
        else
        {
            c.wrongWay = false;
            c.idCarPosPrevius = c.idCarPosCurrent;
            RegisterCar(c, c.completedLaps);

            c.positionCar = ReturnPosCar(c, c.completedLaps);
        }*/
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

    public bool RegisterCar(CarManager car, int vol)
    {
        lapRace[vol].car.Add(car);
        return true;
    }

    public int ReturnPosCar(CarManager car, int vol)
    {
        for (int i = 0; i < lapRace[vol].car.Count; i++)
        {
            if (lapRace[vol].car[i] == car)
            {
                return i + 1;
            }
        }
        return -1;
    }   

    public class Laps
    {
        public List<CarManager> car;

        public Laps()
        {
            car = new List<CarManager>();
        }
    }
}
