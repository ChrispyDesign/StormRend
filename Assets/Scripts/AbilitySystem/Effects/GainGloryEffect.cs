using pokoro.BhaVE.Core.Variables;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	/// <summary>
	/// Gains the specified glory amount
	/// </summary>
    public class GainGloryEffect : Effect
    {
        [SerializeField] int amount = 1;
		[SerializeField] BhaveInt glory;

		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			Debug.Assert(glory, "No glory SOV found!");
			if (glory) glory += amount;
        }
    }
}