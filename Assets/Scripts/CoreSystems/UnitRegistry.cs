using System;
using System.Collections.Generic;
using System.Linq;
using pokoro.Patterns.Generic;
using StormRend.Enums;
using StormRend.MapSystems.Tiles;
using StormRend.States;
using StormRend.Systems.StateMachines;
using StormRend.Utility.Attributes;
using StormRend.Utility.Events;
using UnityEngine;

namespace StormRend.Units
{
	public class UnitRegistry : Singleton<UnitRegistry>
	{
		//Hookups
		/*
		//Not sure about these
		onUnitCreated += PreCalculateMoveTiles.Run
		onUnitKilled += PreCalculateMoveTiles.Run
		*/

		//Inspector
		[Header("Units loaded in automatically. DO NOT load in manually")]
		[ReadOnlyField, SerializeField] List<Unit> _aliveUnits = new List<Unit>();
		[ReadOnlyField, SerializeField] List<Unit> _deadUnits = new List<Unit>();

		//Properties
		public Unit[] aliveUnits => _aliveUnits.ToArray();
		public Unit[] deadUnits => _deadUnits.ToArray();
		public bool allAlliesDead => GetUnitsByType<AllyUnit>().Length <= 0;
		public bool allEnemiesDead => GetUnitsByType<EnemyUnit>().Length <= 0;

		//Events
		[Header("Events")]
		public UnitEvent onUnitCreated;
		public UnitEvent onUnitKilled;

		void Start()
		{
			_deadUnits.Clear();
			FindAllUnits();
		}
		
		//Finds all units and sorts them based on whether they're dead or not
		public void FindAllUnits()
		{
			_aliveUnits = FindObjectsOfType<Unit>().ToList();
			if (_aliveUnits.Count > 0)
				foreach (var a in _aliveUnits)
					if (a.isDead)
						RegisterUnitDeath(a);
		}

	#region Core
		//Adds a new unit to the alive registry
		public void RegisterUnitCreation(Unit u)
		{
			_aliveUnits.Add(u);

			onUnitCreated.Invoke(u);
		}

		//Register's the death of a unit and moves it from the alive to dead list
		public void RegisterUnitDeath(Unit deadUnit)
		{
			if (_aliveUnits.Remove(deadUnit))
			{
				_deadUnits.Add(deadUnit);
				onUnitKilled.Invoke(deadUnit);
			}
			else
				Debug.LogWarningFormat("{0} was not in list of alive units!", deadUnit);
		}

		public T[] GetUnitsByType<T>() where T : Unit => (from u in aliveUnits where u is T select u as T).ToArray();
	#endregion

	#region Turn Enter/Exit logic
		public void RunUnitsBeginTurn(State state)
		{
			var turnState = state as TurnState;
			AnimateUnit[] currentStateUnits = new AnimateUnit[0];
			switch (turnState.unitType)
			{
				case TargetType.Allies:
					currentStateUnits = GetUnitsByType<AllyUnit>();
					break;
				case TargetType.Enemies:
					currentStateUnits = GetUnitsByType<EnemyUnit>();
					break;
			}
			foreach (var u in currentStateUnits)
				u.BeginTurn();
		}
		public void RunUnitsEndTurn(State state)
		{
			var turnState = state as TurnState;
			AnimateUnit[] animateUnits = new AnimateUnit[0];
			switch (turnState.unitType)
			{
				case TargetType.Allies:
					animateUnits = GetUnitsByType<AllyUnit>();
					break;
				case TargetType.Enemies:
					animateUnits = GetUnitsByType<EnemyUnit>();
					break;
			}
			foreach (var u in animateUnits)
				u.EndTurn();
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
		/// Returns true if the unit of specified type is on the tile and out the unit
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
		/// Returns true if the unit is within specified types and is on the tile and out the unit
		/// </summary>
		/// <param name="typesToCheck">List of types to check against</param>
		public static bool TryGetUnitTypeOnTile(Tile tile, out Unit unit, params Type[] typesToCheck)
		{
			var filteredUnits = current.aliveUnits.Where(u => typesToCheck.Contains(u.GetType()));
			// var filteredUnits = current.aliveUnits;
			foreach (var u in filteredUnits)
			{
				if (u.currentTile == tile) 
				{
					// if (typesToCheck.Contains(u.GetType()))
					{
						unit = u;
                        return true;
					}
				}
			}
			unit = null;
			return false;
		}

		/// <summary>
		/// Returns multiple units (WHOOPS! There can only be one unit on a tile. This is useless)
		/// </summary>
		public static bool TryGetUnitTypesOnTile(Tile tile, out Unit[] units, params Type[] typesToCheck)
		{
			//Check they are units
			foreach (var t in typesToCheck)
			{
				if (t.GetType().IsSubclassOf(typeof(Unit)) || t.GetType() == typeof(Unit))
				{
					Debug.LogWarning("Types passed in are not of type 'Unit'. Exiting.");
					units = null;
					return false;
				}
			}

			//Loop through each unit, check if they're standing on the tile, check if they're the right type
			var filteredUnits = current.aliveUnits.Where(u => typesToCheck.Contains(u.GetType()));
			var results = new List<Unit>();
			foreach (var u in filteredUnits)
			{
				if (u.currentTile == tile) 
				{
					results.Add(u);
				}
			}

			//Out results
			if (results.Count > 0)
			{
				units = results.ToArray();
				return true;
			}
			else
			{
				units = null;
				return false;
			}
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