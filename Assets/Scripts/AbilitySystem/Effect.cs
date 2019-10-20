using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities
{
	public abstract class Effect : ScriptableObject
	{
		// public enum Target
		// {
		// 	Self,
		// 	SelectedTiles,
		// 	SelectedTilesWithBreadth,
		// 	AdjacentTiles
		// }

		public bool isFoldOut { get; set; } = true;
		protected UnitRegistry ur;
		Ability owner;

	#region Core
		void OnEnable()
		{
			ur = UnitRegistry.current;
		}

		public void SetOwner(Ability owner)
		{
			this.owner = owner;
		}

		public abstract bool Perform(Unit owner, Tile[] targetTiles);
	#endregion
	}
}