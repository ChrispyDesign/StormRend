using System.Collections;
using System.Linq;
using pokoro.BhaVE.Core.Variables;
using StormRend.MapSystems;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.VisualFX;
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

		[Space(10)]
		[SerializeField] VFX VFX = null;
		[SerializeField] AnimationCurve yPos = AnimationCurve.EaseInOut(0, 1f, 1f, 0.5f);
		[SerializeField] AnimationCurve arriveSpeed = AnimationCurve.EaseInOut(0, 1f, 1f, 0.5f);
		public override bool OnUnitKilled(Ability ability, Unit owner, Unit killedUnit)
		{
			//SEMI-HARDCODE
			var au = owner as AnimateUnit;
			if (killedUnit is EnemyUnit)	//Make sure unit killed is an enemy
			{
				//Calculate soul commune from owner's CURRENT TILE not STARTING TILE ie. once a unit has acted it will lock all team member's position
				var tilesInRange = au.CalculateTargetTiles(ability, owner.currentTile, true);
				if (tilesInRange.Contains(killedUnit.currentTile))
				{
					//Trigger VFX to lerp from killed unit to sage
					owner.StartCoroutine(ReapSoul(owner, killedUnit));
					return true;
				}
			}
			//Killed unit not in range
			return false;
		}

		IEnumerator ReapSoul(Unit owner, Unit killedUnit)
		{
			//Inits
			Vector3 pos;
			float time = 0;
			float rate = 1f / VFX.totalDuration;

			//Create prefab
			Transform instance = VFX.Play(killedUnit.transform.position, killedUnit.transform.rotation).transform;

			//Move VFX
			while (time < 1f)
			{
				time += rate * Time.deltaTime;

				//Towards position
				var tp = arriveSpeed.Evaluate(time);
				pos = Vector3.LerpUnclamped(killedUnit.transform.position, owner.transform.position, tp);

				//Y position
				pos.y = yPos.Evaluate(time);

				//Set position of fx
				if (instance) 	//Keep getting some missing reference exception
					instance.position = pos;

				yield return null;
			}

			//Finally receive the glory; Successful soul reap
			if (glory) glory.value += gloryGainAmount;      //Gain glory
		}
	}
}