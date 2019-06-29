using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topper_Getter : MonoBehaviour
{
    public List<Node_Test> nodes;


    [SerializeField] private LayerMask layerMask;




    void Start()
    {
        foreach (var node in nodes)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(node.transform.position + Vector3.up, Vector3.down, out hit, 3f, layerMask))
            {
                Topper topper = hit.transform.GetComponent<Topper>();
                node.GetComponent<Node_Test>().topper = topper;
                topper.transform.SetParent(node.transform);
            }            

            
        }    
    }
}
