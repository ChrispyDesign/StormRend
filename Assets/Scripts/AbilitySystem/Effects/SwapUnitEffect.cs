/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System.Collections;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.VisualFX;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public class SwapUnitEffect : Effect
	{
		[Tooltip("The particle to be instantiated")]
		[SerializeField] VFX vfx = null;

		[Tooltip("The delay to actually swap the units so that it works visually")]
		[SerializeField] float swapTiming = 3f;

		/// <summary>
		/// Swap or teleport units
		/// </summary>
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			

			//Make sure there is atleast 2 tiles passed in
			if (!(targetTiles.Length >= 2)) { Debug.LogWarning("Not enough target tiles! Exiting..."); return; }
			
			//Try getting animate units if they exist
			UnitRegistry.TryGetAnyUnitOnTile(targetTiles[0], out Unit u0);
			UnitRegistry.TryGetAnyUnitOnTile(targetTiles[1], out Unit u1);
			var au0 = u0 as AnimateUnit;
			var au1 = u1 as AnimateUnit;

			//Play VFXs
			vfx.Play(targetTiles[0].transform.position, targetTiles[0].transform.rotation);
			vfx.Play(targetTiles[1].transform.position, targetTiles[1].transform.rotation);

			//Perform swap at correct time
			owner.StartCoroutine(Swap(au0, au1, targetTiles, swapTiming));
		}

		/// <summary>
		/// Coroutine to correctly time the unit swap 
		/// </summary>
		IEnumerator Swap(AnimateUnit first, AnimateUnit second, Tile[] targetTiles, float delay)
		{
			yield return new WaitForSeconds(delay);

			//Swap/teleport units if they exist
			first?.Move(targetTiles[1], false, false, true);
			second?.Move(targetTiles[0], false, false, true);

			//Reset begin tile
			if (first) first.startTile = first.currentTile;
			if (second) second.startTile = second.currentTile;

			//Recalculate move tiles
			first?.CalculateMoveTiles();
			second?.CalculateMoveTiles();
		}
	}
}