using StormRend.Defunct;
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
		public override bool Perform(Unit owner, Tile[] targetTiles)
        {
			//Make sure there is atleast one tile
			if (targetTiles.Length <= 0) return false;

			//Get the tile
			var t = targetTiles[0];

			//This can only take one target tile so ignore the rest
			if (!UnitRegistry.TryGetAnyUnitOnTile(t, out Unit ignoreMe))
			{
				var au = owner as AnimateUnit;		//Cast
				au.Move(t, false, restrictToMoveTiles);					//Teleport
			}
            return true;
        }
    }
}