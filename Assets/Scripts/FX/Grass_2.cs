using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass_2 : MonoBehaviour
{
    public GameObject boi;


    public void Start()
    {
        Invoke("TurnOn", 0.1f);
    
    }

    void TurnOn()
    {
        boi.SetActive(true);
    }
}
