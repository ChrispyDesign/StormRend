using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass_Main : MonoBehaviour
{
    [SerializeField] GameObject tuft;
    float rand;

    private void Start()
    {
        
        rand = Random.Range(0, 360);

        transform.rotation = Quaternion.Euler(0, rand, 0);

        rand = Random.Range(30f, 60f);

        tuft.transform.localScale = new Vector3(rand / 100, rand / 100, rand / 100);
    }
}
