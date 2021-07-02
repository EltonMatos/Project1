using CarPlayer;
using Network;
using UnityEngine;

public class PitStop : MonoBehaviour
{
    public int idPitStop;

    private void Start()
    {
        var meshRenderer = gameObject.GetComponent<MeshRenderer>();
        var color = GameRoom.Instance.GetColorByLocalGameId(idPitStop);
        meshRenderer.materials[0].color = CarColorManager.Instance.GetColor(color);
    }
}
