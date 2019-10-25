using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		GetComponent<Renderer>().material.SetFloat("_Displacement", Random.Range(0.0f, 2.0f));
    }
}
