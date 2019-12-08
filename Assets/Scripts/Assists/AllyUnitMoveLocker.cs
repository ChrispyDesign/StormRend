/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Linq;
using StormRend.Abilities;
using StormRend.Abilities.Effects;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists 
{ 
	[RequireComponent(typeof(UnitRegistry))]
	public class AllyUnitMoveLocker : MonoBehaviour
	{
		UnitRegistry ur;

		void Awake() => ur = GetComponent<UnitRegistry>();

		void Start()
		{
			foreach (var au in ur.GetAliveUnitsByType<AllyUnit>())
				au.onActed.AddListener(LockMovedUnits);		//Probably don't need to unregister
		}

		/// <summary>
		/// Lock all ally unit movement unless the ability that was just performed has a refresh effect
		/// </summary>
		public void LockMovedUnits(Ability a)
		{
			print("[AllyUnitMoveLocker] Locking moved units");

            //Loop through alive ally units
            foreach (var au in ur.GetAliveUnitsByType<AllyUnit>())		
				//if the unit has moved from it's starting position
				if (au.startTile != au.currentTile)
					//if the ability does NOT have any refresh effects
					if (a.effects.Where(x => x is RefreshEffect).Count() == 0)	
						//Lock the unit's movement
						au.SetCanMove(false);
		}
   	}
}