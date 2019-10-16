using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_Randomiser : MonoBehaviour
{
    public Animator anim;

    public float timer;
    public int rand;

    void Start()
    {
        rand = Random.Range(0, 500);


    }

    private void Update()
    {
        if (timer <= (rand / 100))
        {
            timer += Time.deltaTime;
        }

        if (timer > (rand / 100))
        {
            anim.SetBool("Done_Waiting", true);
        }

    }

}
