using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowTest : MonoBehaviour
{
    public List<Material> snowyBois;

    float scale;

    bool upping = false;

    bool done = true;



    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            if (!upping)
            {
                upping = true;
                done = false;
            }
            else
            {
                upping = false;
                done = false;
            }

            
        }

        if (!done || scale >= 1.0 || scale <= 0.0)
        {
            if (upping)
            {
                scale += Time.deltaTime / 2;
            }
            else
            {
                scale -= Time.deltaTime / 2;
            }

            foreach (var item in snowyBois)            
                item.SetFloat("_SnowOpacity", scale);
            

        }

        if(scale >= 1.0)
        {
            done = true;
            scale = 1;
        }

        if (scale <= 0.0)
        {
            done = true;
            scale = 0;
        }
    }
}
