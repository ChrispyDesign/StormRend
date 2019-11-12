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
	public class PreCalculateMoveTiles : MonoBehaviour
	{
		[EnumFlags, SerializeField] TargetType unitTypes = TargetType.Allies;

		UnitRegistry ur;

		public void Awake()
		{
			ur = UnitRegistry.current;
		}

		public void Run()
		{
			var unitsToCalculateMoveTiles = new List<AnimateUnit>();

			//ALLIES
			if ((unitTypes & TargetType.Allies) == TargetType.Allies)
				unitsToCalculateMoveTiles.AddRange(ur.GetUnitsByType<AllyUnit>());
			//ENEMIES
			if ((unitTypes & TargetType.Enemies) == TargetType.Enemies)
				unitsToCalculateMoveTiles.AddRange(ur.GetUnitsByType<EnemyUnit>());

			//Repopulate 
			foreach (var au in unitsToCalculateMoveTiles)
				au.CalculateMoveTiles();
		}
   	}
}