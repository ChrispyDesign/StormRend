using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities
{
	public abstract class Effect : ScriptableObject
	{
		public bool isFoldOut { get; set; } = true;
		Ability owner;

	#region Core
		public void SetOwner(Ability owner)
		{
			this.owner = owner;
		}

		public abstract void Perform(Unit owner, Tile[] targetTiles);
	#endregion
	}
}

/* Effect Categories
----------------------
Benefits
Curses
Defensive
Offensive
Recovery
Runes
 */

// public enum Target
// {
// 	Self,
// 	SelectedTiles,
// 	SelectedTilesWithBreadth,
// 	AdjacentTiles
// }