using StormRend.Defunct;
using StormRend.MapSystems.Tiles;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
    public class TeleportEffect : Effect
    {
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
				au.Move(t, false);					//Teleport
			}
            return true;
        }
    }
}