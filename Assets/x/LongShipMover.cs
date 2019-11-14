using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LongShipMover : MonoBehaviour
{
    [SerializeField] Vector2 speedLimit;
    [SerializeField] float speed;

    [SerializeField] Transform target;
    [SerializeField] Vector3 direction;

    [SerializeField] List<Transform> LSTargets;
    

    // Start is called before the first frame update
    void Start()
    {
        target = LSTargets[0];
        NewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - target.position).sqrMagnitude > (.3f))
        {
            direction = target.position - transform.position;

            transform.LookAt(target.position);
            transform.position += direction.normalized * speed * Time.deltaTime; 
        }
        else
        {
            NewTarget();
        }
    }


    void NewTarget()
    {
        int rand = Random.Range(0, LSTargets.Count);

        if (LSTargets[rand] == target)
        {
            NewTarget();            
        }
        else
        {
            target = LSTargets[rand];
            speed = Random.Range(speedLimit.x, speedLimit.y);
            transform.LookAt(target.position);
        }
    }    
}
