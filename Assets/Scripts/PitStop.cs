using System;
using System.Collections;
using System.Collections.Generic;
using CarPlayer;
using Network;
using Photon.Pun;
using UnityEngine;

public class PitStop : MonoBehaviour
{
    public Transform posCarPitStop;

    public int idPitStop;


    private void Start()
    {
        var meshRenderer = gameObject.GetComponent<MeshRenderer>();
        var color = GameRoom.Instance.GetColorByLocalGameId(idPitStop);
        meshRenderer.materials[0].color = CarColorManager.Instance.GetColor(color);
    }
}
