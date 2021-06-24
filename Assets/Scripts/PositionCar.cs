using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCar : MonoBehaviour
{
    private CarManager c;

    private CheckCar check;

    public List<Laps> lapRace;

    public int idPosCurrent;

    private void Start()
    {
        lapRace = new List<Laps>();
        check = new CheckCar();
        for (int i = 0; i < GameManager.Instance.lapsMax; i++)
        {
            lapRace.Add(new Laps());
        }
    }

    private void Update()
    {
        //StartCoroutine(CheckPosition());
    }

    /*IEnumerator CheckPosition()
    {
        yield return new WaitForSeconds(1);
        c.positionCar = ReturnPosCar(c, c.completedLaps);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        CarManager c = other.transform.root.GetComponent<CarManager>();
        c.idCarPosCurrent = idPosCurrent;
        Debug.Log("idCar: " + c.idCarPosCurrent + " idCarPosPrevius: " + c.idCarPosPrevius);
        if (c.idCarPosCurrent < c.idCarPosPrevius && idPosCurrent != 1)
        {
            UiManager.Instance.wrongWayShow = true;
            c.idCarPosPrevius = c.idCarPosCurrent;
        }
        else
        {
            UiManager.Instance.wrongWayShow = false;
            c.idCarPosPrevius = c.idCarPosCurrent;
            RegisterCar(c, c.completedLaps);
            c.positionCar = ReturnPosCar(c, c.completedLaps);
        }        
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
