using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] private GameObject grassTuft;
    [SerializeField] private List<Material> grass_mat;



    private void Awake()
    {
        int rand = Random.Range(0, 360);                                                                    //Sets a random Number
        transform.rotation = Quaternion.Euler(0, rand, 0);                                                  //Rotates The Tuft Randomly

        rand = Random.Range(80, 120);                                                                       //Sets a random Number
        grassTuft.transform.localScale = new Vector3((rand/100), (rand / 100), (rand / 100));               //Scales The Tuft Randomly

        rand = Random.Range(0, grass_mat.Count);

        grassTuft.GetComponent<Renderer>().material = grass_mat[rand];
    }
}
