using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField] private GameObject grassTuft;
    [SerializeField] private Vector2 speed;

    private void Awake()
    {        
        int rand = Random.Range(0, 360);                                                                    //Sets a random Number
        transform.rotation = Quaternion.Euler(0, rand, 0);                                                  //Rotates The Tuft Randomly


        grassTuft.GetComponent<Renderer>().material.SetFloat("_Speed", Random.Range(speed.x, speed.y));     //Randomises the speed of the grass sway

        rand = Random.Range(0, 2);                                                                          //Sets a random Number
        if (rand >= 1)                                                                                      //Changers The Sway Direction
            grassTuft.GetComponent<Renderer>().material.SetFloat("_Direction", 1);
        if (rand < 1)
            grassTuft.GetComponent<Renderer>().material.SetFloat("_Direction", -1);
    }
}
