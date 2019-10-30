using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    [SerializeField] List<Transform> nodes = new List<Transform>();
    [SerializeField] Transform target;
    [SerializeField] Transform start;

    [SerializeField] float speed = 1;
    [SerializeField] float percent = 1;


    void Start()
    {
        NewNode();
    }

    void Update()
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
