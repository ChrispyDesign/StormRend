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
				au.onActed.AddListener(Lock);
		}

		/// <summary>
		/// Lock all ally unit movement unless the ability that was just performed has a refresh effect
		/// </summary>
		public void Lock(Ability a)
		{
			Debug.Log("Locking Units!");
			foreach (var au in ur.GetAliveUnitsByType<AllyUnit>())
				if (a.effects.Where(x => x is RefreshEffect).Count() == 0)	//Where there aren't any refresh effects
					au.SetCanMove(false);	//Lock unit movement
		}
   	}
}