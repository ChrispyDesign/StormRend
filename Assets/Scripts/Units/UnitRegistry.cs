
using System.Collections.Generic;
using System.Linq;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Units
{
	public class UnitRegistry : MonoBehaviour
	{
		//Privates
		[Header("Auto load units upon start. Do not load in manually")]
		[ReadOnlyField, SerializeField] List<Unit> aliveUnits = new List<Unit>();
		[ReadOnlyField, SerializeField] List<Unit> deadUnits = new List<Unit>();

		void Start()
		{
			FindAllUnits();
		}

		void FindAllUnits()
		{
			aliveUnits = FindObjectsOfType<Unit>().ToList();
		}

		//Getters
		public Unit[] GetUnits<T>() where T : Unit => aliveUnits.Where(x => (x is T)).ToArray();

		//Core
		public void RegisterDeath(Unit deadUnit)
		{
			if (aliveUnits.Remove(deadUnit))
			{
				deadUnits.Add(deadUnit);
			}
			else
			{
				Debug.LogWarningFormat("{0} was not in list of alive units!", deadUnit);
			}
		}
   	}
}