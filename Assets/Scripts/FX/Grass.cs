using System.Collections.Generic;
using UnityEngine;

namespace StormRend.VisualFX
{
    public class Grass : MonoBehaviour
    {
        [SerializeField] int maxRandomRotation = 360;
        [SerializeField] int minRandomScale = 60;
        [SerializeField] int maxRandomScale = 140;
        [SerializeField] List<Material> grassMaterials = new List<Material>();

        void Start()
        {
            //Transforms
            transform.rotation = Quaternion.Euler(0, Random.Range(0, maxRandomRotation), 0);                                           //Rotates The Tuft Randomly
            float randomScale = Random.Range(minRandomScale, maxRandomScale);                                                                       //Sets a random Number
            transform.localScale = new Vector3((randomScale / 100), (randomScale / 100), (randomScale / 100));               //Scales The Tuft Randomly

            //Materials
            if (grassMaterials.Count > 0)
            {
                var randomMaterialIDX = Random.Range(0, grassMaterials.Count);
                GetComponentInChildren<Renderer>().material = grassMaterials[randomMaterialIDX];
            }
        }
    }
}
