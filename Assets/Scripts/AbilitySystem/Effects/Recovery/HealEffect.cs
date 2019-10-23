﻿using StormRend.Defunct;
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
		public override bool Perform(Unit owner, Tile[] targetTiles)
		{
			foreach (var t in targetTiles)
			{
				if (UnitRegistry.TryGetUnitOnTile(t, out Unit u))		//Try getting a unit on top
				{
					u.HP += healAmount;		//Heal
				}
			}
			return true;
		}
    }
}