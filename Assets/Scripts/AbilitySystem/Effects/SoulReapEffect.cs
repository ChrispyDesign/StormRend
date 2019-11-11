using System.Linq;
using pokoro.BhaVE.Core.Variables;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	/// <summary>
	/// If owner is within cast area of unit's death then 
	/// </summary>
	public class SoulReapEffect : PassiveEffect
	{
		[SerializeField] int gloryGainAmount = 1;
		[SerializeField] BhaveInt glory = null;
		public override void OnUnitKilled(Ability ability, Unit owner, Unit killedUnit)
		{
			Debug.Log("OnUnitKilled");
			//HARDCODE
			var au = owner as AnimateUnit;
			if (killedUnit is EnemyUnit)	//Make sure unit killed is an enemy
			{
				//Make sure unit is in range
				var tilesInRange = au.CalculateTargetTiles(ability, false);
				if (tilesInRange.Contains(killedUnit.currentTile))
				{
					if (glory) glory.value += gloryGainAmount;		//Gain glory
				}
			}
		}
	}
}