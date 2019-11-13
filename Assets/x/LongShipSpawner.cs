using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LongShipSpawner : MonoBehaviour
{
    public List<LongShipSpawner> LSSpawners = new List<LongShipSpawner>();

    // Start is called before the first frame update
    void Start()
    {
        LSSpawners = FindObjectsOfType<LongShipSpawner>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
