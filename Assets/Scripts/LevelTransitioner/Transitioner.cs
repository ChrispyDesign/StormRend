﻿using StormRend.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitioner : MonoBehaviour
{
    [SerializeField] List<AllyUnit> allyUnits = new List<AllyUnit>();
    [SerializeField] List<EnemyUnit> enemyUnits = new List<EnemyUnit>();

    private void Awake()
    {
        allyUnits.AddRange(FindObjectsOfType<AllyUnit>());
        enemyUnits.AddRange(FindObjectsOfType<EnemyUnit>());
    }

	private void Update()
	{
		
	}

	public void CheckEnemyDead()
	{
		int enemyDied = 0;

		foreach (EnemyUnit enemy in enemyUnits)
		{
			if (enemy.isDead)
				enemyDied++;
		}

		if (enemyUnits.Count == enemyDied)
		{
			
		}
	}
}
