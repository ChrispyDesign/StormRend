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
				//NOTE: This detect killed units within range of current unit position
				//Could potentially cheat with this
				// var tilesInRange = au.CalculateTargetTiles(ability, false);

				//NOTE: this detects killed units within range of owners start of turn position
				//The soul commune display has to stay in place?
				var tilesInRange = au.CalculateTargetTiles(ability, au.startTile, true);
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
			var instance = VFX.Play(killedUnit.transform.position, killedUnit.transform.rotation);

			while (time < 1f)
			{
				time += rate * Time.deltaTime;

				//Towards position
				var tp = arriveSpeed.Evaluate(time);
				Debug.Log("Towards Path: " + tp);
				pos = Vector3.Lerp(killedUnit.transform.position, owner.transform.position, tp);

				//Y position
				pos.y = yPos.Evaluate(time);

				//Set position of fx
				instance.transform.position = pos;
				yield return null;
			}

			//Finally receive the glory; Successful soul reap
			if (glory) glory.value += gloryGainAmount;      //Gain glory
		}
	}
}