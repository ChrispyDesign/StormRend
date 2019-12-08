/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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
