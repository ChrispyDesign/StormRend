using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    /// <summary>
    /// Teleport owner to a tile. This effect only requires one tile
    /// </summary>
    public class TeleportEffect : Effect
    {
		[Tooltip("Can only teleport to tiles that can be pathfound to")]
		[SerializeField] bool restrictToMoveTiles = false;
		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
        {
			//Make sure there is atleast one tile
			if (targetTiles.Length <= 0) { Debug.LogWarning("Not enough target tiles! Exiting..."); return; }

			//Get the tile
			var t = targetTiles[0];

			//This can only take one target tile so ignore the rest
			if (!UnitRegistry.TryGetAnyUnitOnTile(t, out Unit ignoreMe))
			{
				var au = owner as AnimateUnit;		//Cast
				au.Move(t, false, restrictToMoveTiles, true);	//Teleport

				//If not restricted to move tiles then reset begin/current tiles and recalculate moves
				if (!restrictToMoveTiles)
				{
					au.startTile = au.currentTile;
					au.CalculateMoveTiles();
				}
			}
        }
    }
}