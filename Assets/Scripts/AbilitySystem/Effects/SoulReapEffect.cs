using System.Linq;
using pokoro.BhaVE.Core.Variables;
using StormRend.MapSystems;
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
		public override bool OnUnitKilled(Ability ability, Unit owner, Unit killedUnit)
		{
			//SEMI-HARDCODE
			var au = owner as AnimateUnit;
			if (killedUnit is EnemyUnit)	//Make sure unit killed is an enemy
			{
				//NOTE: This detect killed units within range of current unit position
				//Could potentially cheat with this
				// var tilesInRange = au.CalculateTargetTiles(ability, false);

				//NOTE: this detects killed units within range of owners start of turn position
				//The soul commune display has to stay in place?
				var tilesInRange = au.CalculateTargetTiles(ability, au.beginTurnTile, true);
				if (tilesInRange.Contains(killedUnit.currentTile))
				{
					//Successful soul reap
					if (glory) glory.value += gloryGainAmount;		//Gain glory
					return true;
				}
			}
			//Killed unit not in range
			return false;
		}
	}
}