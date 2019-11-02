﻿using System;
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

        [EnumFlags, SerializeField] RefreshType refreshType = 0;
		[SerializeField] int allowedRefreshes = 1;
		[ReadOnlyField, SerializeField] int refreshCount = 0;		//internal refresh count

		public override void Prepare(Ability ability, Unit owner)
		{
			refreshCount = 0;	//Reset the refresh count
		}

		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
        {
			if (refreshCount >= allowedRefreshes) return;
			
			//MoveAgain
			if ((refreshType & RefreshType.MoveAgain) == RefreshType.MoveAgain)
				(owner as AnimateUnit).SetCanMove(true);     //You should always be able to move again right?

			//ActAgain
			if ((refreshType & RefreshType.ActAgain) == RefreshType.ActAgain)
				(owner as AnimateUnit).SetCanAct(true);     //You should always be able to move again right?

			//Inc count
			refreshCount++;
        }
    }
}