using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LongShipMover : MonoBehaviour
{
    public List<LongShipTarget> LSTargets = new List<LongShipTarget>();
    public float speed;
    public float speedMin = 0.1f;
    public float speedMax = 2f;
    // public Transform target;
    

    // Start is called before the first frame update
    void Start()
    {
        LSTargets = FindObjectsOfType<LongShipTarget>().ToList();        // populate list
        ChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        // move forward
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    // called when the LongShip reaches its target
    public void ChangeDirection()
    {
        Debug.Log("Ahoy");
        speed = Random.Range(speedMin, speedMax);                         // assign random speed
        int targetIndex = Random.Range(0,LSTargets.Count-1);             // create a random number equal to the amount of targets in the list
        transform.LookAt(LSTargets[targetIndex].transform.position);     // make the ship look at one of the targets
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LSTarget")
        {
            Debug.Log("you touched the butt");
            ChangeDirection();
        }
    }
}
