using System.Collections.Generic;
using UnityEngine;

namespace CarPlayer
{
    public class CarColorManager : MonoBehaviour
    {
        public static CarColorManager Instance;

        public Mesh[] meshes;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public Color GetColor(CarColors color)
        {
            switch (color)
            {
                case CarColors.Orange:
                    return new Color(255f/255f, 191f/255f, 128f/255f);
                case CarColors.Green:
                    return new Color(191f/255f, 255f/255f, 128f/255f);
                case CarColors.Black:
                    return new Color(71f/255f, 86f/255f, 107f/255f);
                case CarColors.Blue:
                    return new Color(128f/255f, 179f/255f, 255f/255f);
                case CarColors.Purple:
                    return new Color(213f/255f, 128f/255f, 255f/255f);
                case CarColors.Red:
                    return new Color(255f/255f, 128/255f, 128f/255f);
                default:
                    return Color.white;
            }
        }

        public Mesh GetMesh(CarColors color)
        {
            Mesh colorMesh = new Mesh();
            foreach (var mesh in meshes)
            {
                if ($"SR_Veh_StockCar_{color}" == mesh.name)
                {
                    colorMesh = mesh;
                }
            }
            return colorMesh;
        }
    }
}