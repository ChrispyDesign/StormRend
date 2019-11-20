using System;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public class RefreshEffect : Effect
    {
		[Flags]
		public enum RefreshType 
		{
			MoveAgain = 1 << 0,
			ActAgain = 1 << 1,
		}

		[Tooltip("The duration before the unit gets re-selected")]
		[SerializeField] float delay = 1f;

		[Header("NOTE: Refresh must be after Damage Effect")]
		[ReadOnlyField, SerializeField] int refreshCount = 0;		//internal refresh count
		[SerializeField] bool onlyIfHaveKilledThisTurn = false;
        [EnumFlags, SerializeField] RefreshType refreshType = 0;

		[SerializeField] int allowedRefreshes = 1;

		public override void Prepare(Ability ability, Unit owner) => refreshCount = 0;
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
        {
			if (refreshCount >= allowedRefreshes) return;

			//Must have killed a unit to allow this refresh to continue
			if (onlyIfHaveKilledThisTurn && !owner.hasKilledThisTurn) return;

			Debug.Log("Refreshing!");
			
			//MoveAgain
			if ((refreshType & RefreshType.MoveAgain) == RefreshType.MoveAgain)
				(owner as AnimateUnit).SetCanMove(true, delay);     //You should always be able to move again right?

			//ActAgain
			if ((refreshType & RefreshType.ActAgain) == RefreshType.ActAgain)
				(owner as AnimateUnit).SetCanAct(true, delay);     //You should always be able to move again right?

			//Inc count
			refreshCount++;
        }
    }
}