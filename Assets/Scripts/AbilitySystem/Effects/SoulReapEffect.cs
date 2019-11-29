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
				var tilesInRange = ability.GetTargetTiles(au.currentTile);
				if (tilesInRange.Contains(killedUnit.currentTile))
				{
					//Trigger VFX to lerp from killed unit to sage
					owner.StartCoroutine(ReapSoul(owner, killedUnit));	//Ima Genius!
					return true;
				}
			}
			//Killed unit not in range
			return false;
		}

		IEnumerator ReapSoul(Unit owner, Unit killedUnit)
		{
			//Inits
			Vector3 vfxPos;
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
				vfxPos = Vector3.Lerp(killedUnit.transform.position, owner.transform.position, tp);

				//Y position relative to the owner's position
				vfxPos.y = owner.transform.position.y + yPos.Evaluate(time);

				//Set position of fx
				if (instance) 	//Keep getting some missing reference exception
					instance.position = vfxPos;

				yield return null;
			}

			//Finally receive the glory; Successful soul reap
			if (glory) glory.value += gloryGainAmount;      //Gain glory
		}
	}
}