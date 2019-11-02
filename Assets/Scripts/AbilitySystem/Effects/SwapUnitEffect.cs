using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public class SwapUnitEffect : Effect
	{
		/// <summary>
		/// Swap units. Target Tile types should be set to Animate Objects or maybe even Inanimate
		/// </summary>
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			//Make sure there is atleast 2 tiles passed in
			if (!(targetTiles.Length >= 2)) { Debug.LogWarning("Not enough target tiles! Exiting..."); return; }

			//Make sure the tiles have units on them
			foreach (var t in targetTiles)
				if (!UnitRegistry.IsAnyUnitOnTile(t)) return;

			//Perform swap
			UnitRegistry.TryGetAnyUnitOnTile(targetTiles[0], out Unit u1);
			UnitRegistry.TryGetAnyUnitOnTile(targetTiles[1], out Unit u2);
			var au1 = u1 as AnimateUnit;
			var au2 = u2 as AnimateUnit;
			Tile u1tile;
			u1tile = u1.currentTile;
			au1.Move(au2.currentTile, false, false, true);
			au2.Move(u1tile, false, false, true);
		}
	}
}