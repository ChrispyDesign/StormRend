using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Pannels : MonoBehaviour
{
    [SerializeField] List<GameObject> textboxes;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            textboxes[0].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            textboxes[1].gameObject.SetActive(true);
            textboxes[0].GetComponent<Animator>().SetInteger("Amount", 1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            textboxes[2].gameObject.SetActive(true);
            textboxes[1].GetComponent<Animator>().SetInteger("Amount", 1);
            textboxes[0].GetComponent<Animator>().SetInteger("Amount", 2);
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            foreach (var item in textboxes)
            {
                item.GetComponent<Animator>().SetBool("Done", true);
                
            }
        }
    }
}
