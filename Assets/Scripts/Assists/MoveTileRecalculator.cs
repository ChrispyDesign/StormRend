/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Collections.Generic;
using StormRend.Enums;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Assists 
{ 
	/// <summary>
	/// Required at the start of the player's turn so that the player can hover over an enemy unit at see it's range
	/// Also required to run when a unit is created or killed
	/// </summary>
	public class MoveTileRecalculator : MonoBehaviour
	{
		[EnumFlags, SerializeField] TargetType unitTypes = TargetType.Allies;

		UnitRegistry ur;

		public void Awake()
		{
			ur = UnitRegistry.current;
		}
		void OnEnable()
		{
			ur.onUnitCreated.AddListener(OnUnitRegistryChanged);
			ur.onUnitKilled.AddListener(OnUnitRegistryChanged);
		}
		void OnDisable()
		{
			ur.onUnitCreated.RemoveListener(OnUnitRegistryChanged);
			ur.onUnitKilled.RemoveListener(OnUnitRegistryChanged);
		}

		void OnUnitRegistryChanged(Unit unit)
		{
			Recalculate();
		}

		public void Recalculate()
		{
			var unitsToCalculateMoveTiles = new List<AnimateUnit>();

			//ALLIES
			if ((unitTypes & TargetType.Allies) == TargetType.Allies)
				unitsToCalculateMoveTiles.AddRange(ur.GetAliveUnitsByType<AllyUnit>());
			//ENEMIES
			if ((unitTypes & TargetType.Enemies) == TargetType.Enemies)
				unitsToCalculateMoveTiles.AddRange(ur.GetAliveUnitsByType<EnemyUnit>());

			//Repopulate 
			foreach (var au in unitsToCalculateMoveTiles)
				au.CalculateMoveTiles();
		}

		public void Recalculate<T>() where T : AnimateUnit
		{
			foreach (var au in ur.GetAliveUnitsByType<T>())
				au.CalculateMoveTiles();
		}
   	}
}