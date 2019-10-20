using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    [SerializeField] private List<Transform> nodes;
    [SerializeField] private Transform target;
    [SerializeField] private Transform start;

    [SerializeField] private float speed;
    [SerializeField] private float percent;


    private void Start()
    {
        NewNode();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, target.position) <= 0.01f)        
            NewNode();

        percent += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(start.position, target.position, percent);
    }

    void NewNode()
    {
        start = transform;

        int rand = Random.Range(0, nodes.Count);

        if (nodes[rand] == target)
            NewNode();
        else        
            target = nodes[rand];

    }
}
