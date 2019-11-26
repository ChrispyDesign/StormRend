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
				au.onActed.AddListener(LockMovedUnits);
		}

		/// <summary>
		/// Lock all ally unit movement unless the ability that was just performed has a refresh effect
		/// </summary>
		public void LockMovedUnits(Ability a)
		{
            //Loop through alive ally units
            foreach (var au in ur.GetAliveUnitsByType<AllyUnit>())		
				//if the unit has moved from it's starting position
				if (au.startTile != au.currentTile)
					//if the ability does NOT have any refresh effects
					if (a.effects.Where(x => x is RefreshEffect).Count() == 0)	
						//Lock the unit's movement
						au.SetCanMove(false);
		}
   	}
}