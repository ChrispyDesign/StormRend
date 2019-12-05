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
		[SerializeField] int allowedRefreshes = 1;
        [EnumFlags, SerializeField] RefreshType refreshType = 0;
		[SerializeField] bool onlyIfHaveKilled = false;
		[SerializeField] bool allowChainRefreshes = true;

		public override void Prepare(Ability ability, Unit owner) => refreshCount = 0;
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
        {
			if (refreshCount >= allowedRefreshes) return;

			//Must have killed a unit to allow this refresh to continue
			if (onlyIfHaveKilled && !owner.hasJustKilled) return;

			//Only increment count if not set only on kill and if chaining is not allowed ???
			if (!allowChainRefreshes)
				refreshCount++;

			//"Use" the killed flag so that you can't keep chaining if you've only killed once
			if (onlyIfHaveKilled)
				owner.hasJustKilled = false;

			// Debug.Log("Refreshing!");
			
			//MoveAgain
			if ((refreshType & RefreshType.MoveAgain) == RefreshType.MoveAgain)
				(owner as AnimateUnit).SetCanMove(true, delay);     //You should always be able to move again right?

			//ActAgain
			if ((refreshType & RefreshType.ActAgain) == RefreshType.ActAgain)
				(owner as AnimateUnit).SetCanAct(true, delay);     //You should always be able to move again right?
        }
    }
}