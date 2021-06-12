using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitStop : MonoBehaviour
{
    public Transform posCarPitStop;

    public int idPitStop;

    public Material color;

    public PlayerCar car;

    private void Start()
    {
        //color = GetComponent<Material>();
    }

    void ChangeColorPit()
    {
        if(idPitStop == car.idCar)
        {
            // trocar cor do pit conforme a cor do carro
        }
    }

}
