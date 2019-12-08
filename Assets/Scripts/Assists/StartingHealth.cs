/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
	/// <summary>
	/// Class to automatically set a unit to a certain starting health
	/// </summary>
	public class StartingHealth : MonoBehaviour
	{
		[SerializeField] int startingHealth = 2;
		Unit unit = null;

		void Awake()
		{
			unit = GetComponent<Unit>();

			Debug.Assert(unit, "No unit found! Disabling..");
			if (!unit) enabled = false;
		}

		void Start()
		{
			unit.HP = startingHealth;
			var nullHealthData = new HealthData(null, 0);
			unit.onTakeDamage.Invoke(nullHealthData);
		}
	}
}