
using System;
using System.Collections.Generic;
using System.Linq;
using pokoro.Patterns.Generic;
using StormRend.MapSystems.Tiles;
using StormRend.States;
using StormRend.Systems.StateMachines;
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
			_deadUnits.Clear();
			FindAllUnits();
		}
		
		//Finds all units and sorts them based on whether they're dead or not
		public void FindAllUnits()
		{
			_aliveUnits = FindObjectsOfType<Unit>().ToList();
			foreach (var a in _aliveUnits)
				if (a.isDead)
					RegisterDeath(a);
		}

	#region Core
		//Adds a new unit to the alive registry
		public void RegisterUnit(Unit u) => _aliveUnits.Add(u);
		public T[] GetUnitsByType<T>() where T : Unit => _aliveUnits.Where(x => (x is T)).ToArray() as T[];
		//Register's the death of a unit and moves it from the alive to dead list
		public void RegisterDeath(Unit deadUnit)
		{
			if (_aliveUnits.Remove(deadUnit))
				_deadUnits.Add(deadUnit);
			else
				Debug.LogWarningFormat("{0} was not in list of alive units!", deadUnit);
		}
	#endregion

	#region Status Effect Runners
		public void RunStatusEffectOnTurnEnter(State state)
		{
			switch (state)
			{
				case AllyTurnState allyTurnState:
					var allies = GetUnitsByType<AllyUnit>();
					foreach (var a in allies)
						a.BeginTurn();
					break;
				case EnemyTurnState enemyTurnState:
					var enemies = GetUnitsByType<EnemyUnit>();
					foreach (var e in enemies)
						e.BeginTurn();
					break;
			}
		}
		public void RunStatusEffectsOnTurnExit(State state)
		{
			switch (state)
			{
				case AllyTurnState allyTurnState:
					var allies = GetUnitsByType<AllyUnit>();
					foreach (var a in allies)
						a.EndTurn();
					break;
				case EnemyTurnState enemyTurnState:
					var enemies = GetUnitsByType<EnemyUnit>();
					foreach (var e in enemies)
						e.EndTurn();
					break;
			}
		}
	#endregion

	#region OnTile Utility Functions
		/// <summary>
		/// Returns true if ANY unit is on tje tile. Slightly more efficient than TryGetUnitTypeOnTile<>()
		/// </summary>
		public static bool IsAnyUnitOnTile(Tile tile)
		{
			foreach (var u in current.aliveUnits)
				if (u.currentTile == tile)
					return true;
			return false;
		}

		/// <summary>
		/// Returns true and outputs unit if ANY unit is on a the tile
		/// </summary>
		public static bool TryGetAnyUnitOnTile(Tile tile, out Unit unit)
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
		public static bool TryGetUnitTypeOnTile<T>(Tile tile, out T unit) where T : Unit
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
		/// Returns true if any unit or any unit derived from one of the types passed in is standing on the tile
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