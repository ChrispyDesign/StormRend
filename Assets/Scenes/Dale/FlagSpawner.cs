using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    public GameObject playerFlag;
    public GameObject enemyFlag;

    public bool playersTurn = true;

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playersTurn = !playersTurn;

            if (playersTurn)
            {
                playerFlag.SetActive(true);
                enemyFlag.SetActive(false);
            }
            else
            {
                playerFlag.SetActive(false);
                enemyFlag.SetActive(true);
            }
        }
                

    }
}
