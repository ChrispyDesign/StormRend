using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Abilities
{
	public abstract class Effect : ScriptableObject
	{
		public bool isFoldOut { get; set; } = true;

	#region Core
		public virtual void Prepare(Ability ability, Unit owner) { Debug.LogFormat("{0}.Effect.Prepare()", this.name); }
		public abstract void Perform(Ability ability, Unit owner, Tile[] targetTiles);
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