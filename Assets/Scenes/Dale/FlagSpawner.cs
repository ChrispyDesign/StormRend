using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    public GameObject playerFlag;
    public GameObject enemyFlag;

    public GameObject currentFlag;

    public bool playersTurn = false;

    public void playerTurn()
    {
        playersTurn = true;
        if (currentFlag != null)
        Destroy(currentFlag, 1.0f);

        currentFlag = Instantiate(playerFlag, transform);
        currentFlag.SetActive(true);
    }

    public void enemyTurn()
    {
        playersTurn = false;
        if (currentFlag != null)
            Destroy(currentFlag, 1.0f);

        currentFlag = Instantiate(enemyFlag, transform);
        currentFlag.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            if (!playersTurn)
                playerTurn();
            else
                enemyTurn();

    }
}
