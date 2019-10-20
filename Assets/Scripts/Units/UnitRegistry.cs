
using System.Collections.Generic;
using System.Linq;
using pokoro.Patterns.Generic;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Units
{
	public class UnitRegistry : Singleton<UnitRegistry>
	{
		//Inspector
		[Header("Auto load units upon start. Do not load in manually")]
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
		public T[] GetUnits<T>() where T : Unit => _aliveUnits.Where(x => (x is T)).ToArray() as T[];
		public Unit[] GetAllAliveUnits() => aliveUnits.ToArray();

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
	}
}