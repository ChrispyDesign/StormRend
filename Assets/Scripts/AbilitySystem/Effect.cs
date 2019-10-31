using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Abilities
{
	public abstract class Effect : ScriptableObject
	{
		public bool isFoldOut { get; set; } = true;
		[ReadOnlyField, SerializeField] protected Ability container;
		[ReadOnlyField, SerializeField] protected Unit owner;

	#region Core
		public virtual void Prepare(Unit owner) { this.owner = owner; }
		public abstract void Perform(Unit owner, Tile[] targetTiles);
		public void SetContainer(Ability container) => this.container = container;
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