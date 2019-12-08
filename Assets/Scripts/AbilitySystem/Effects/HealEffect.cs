/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public class HealEffect : Effect
    {
        [SerializeField] int healAmount = 1;

		/// <summary>
		/// Heal any units that are on the passed in tiles
		/// </summary>
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			foreach (var t in targetTiles)
			{
				if (UnitRegistry.TryGetAnyUnitOnTile(t, out Unit u))		//Try getting a unit on top
				{
					u.Heal(new HealthData(owner, healAmount));
				}
			}
		}
    }
}