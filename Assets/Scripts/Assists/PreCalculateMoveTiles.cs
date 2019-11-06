using System.Collections.Generic;
using StormRend.Enums;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Assists 
{ 
	/// <summary>
	/// Required at the start of the player's turn so that the player can hover over an enemy unit at see it's range
	/// </summary>
	public class PreCalculateMoveTiles : MonoBehaviour
	{
		[SerializeField] TargetType unitType;

		UnitRegistry ur;

		public void Awake()
		{
			ur = UnitRegistry.current;
		}

		public void Run()
		{
			AnimateUnit[] animateUnits = null;
			switch (unitType)
			{
				case TargetType.Allies:
					animateUnits = ur.GetUnitsByType<AllyUnit>(); break;
				case TargetType.Enemies:
					animateUnits = ur.GetUnitsByType<EnemyUnit>(); break;
			}
			foreach (var au in animateUnits)
				au.CalculateMoveTiles();
		}
   	}
}