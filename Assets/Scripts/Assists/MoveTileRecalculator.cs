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