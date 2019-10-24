
using System;
using System.Collections.Generic;
using System.Linq;
using pokoro.Patterns.Generic;
using StormRend.MapSystems.Tiles;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Units
{
	public class UnitRegistry : Singleton<UnitRegistry>
	{
		//Inspector
		[Header("Units loaded in automatically. DO NOT load in manually")]
		[ReadOnlyField, SerializeField] List<Unit> _aliveUnits = new List<Unit>();
		[ReadOnlyField, SerializeField] List<Unit> _deadUnits = new List<Unit>();

		//Properties
		public Unit[] aliveUnits => _aliveUnits.ToArray();
		public Unit[] deadUnits => _deadUnits.ToArray();

		void Start()
		{
			FindAllUnits();

			void FindAllUnits()
			{
				_aliveUnits = FindObjectsOfType<Unit>().ToList();
			}
		}

	#region Core
		public void RegisterUnit(Unit u) => _aliveUnits.Add(u);
		public T[] GetUnitsByType<T>() where T : Unit => _aliveUnits.Where(x => (x is T)).ToArray() as T[];
		public void RegisterDeath(Unit deadUnit)
		{
			if (_aliveUnits.Remove(deadUnit))
			{
				_deadUnits.Add(deadUnit);
			}
			else
			{
				Debug.LogWarningFormat("{0} was not in list of alive units!", deadUnit);
			}
		}
	#endregion

	#region On Tile Utility Functions
		/// <summary>
		/// Check if there's a unit on the tile. If so return true and output the unit
		/// </summary>
		public static bool TryGetUnitOnTile(Tile tile, out Unit unit)
		{
			foreach (var u in current.aliveUnits)
				if (u.currentTile == tile)
				{
					unit = u;
					return true;
				}
			unit = null;
			return false;
		}

		/// <summary>
		/// Check if the unit of specified type is on the tile
		/// </summary>
		public static bool IsUnitTypeOnTile<T>(Tile tile, out T unit) where T : Unit
		{
			var filteredUnits = current.aliveUnits.Where(x => x is T);
			foreach (var u in filteredUnits)
			{
				if (u.currentTile == tile) 
				{
					unit = u as T;
					return true;
				}
			}
			unit = null;
			return false;
		}

		/// <summary>
		/// Returns true if any unit is standing on tile and also is or is derived from one of the typesToCheck
		/// </summary>
		/// <returns></returns>
		public static bool AreUnitTypesOnTile(Tile tile, params Type[] typesToCheck)
		{
			//Check they are units
			foreach (var t in typesToCheck)
			{
				if (t.GetType().IsSubclassOf(typeof(Unit)) || t.GetType() == typeof(Unit))
				{
					Debug.LogWarning("Types passed in are not of type 'Unit'. Exiting.");
					return false;
				}
			}

			foreach (var u in current.aliveUnits)
			{
				//Check if unit is on tile first
				if (u.currentTile == tile)
				{
					//...and if it's of type or is derived from the list of types to check
					foreach (var type in typesToCheck)
					{
						if (u.GetType().IsSubclassOf(type) || (u.GetType() == type))
							return true;
					}
				}
			}
			return false;
		}
	#endregion
	}
}