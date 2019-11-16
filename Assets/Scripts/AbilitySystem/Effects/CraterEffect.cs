using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities
{
	/// <summary>
	/// Pushes the target tile down, causing a crater to form
	/// </summary>
	public class CraterEffect : Effect
	{
		[SerializeField] float amount = 0.2f;

		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			foreach (var tt in targetTiles)
			{
				var pos = tt.transform.position;
				pos.y -= amount;
				tt.transform.position = pos;
			}
		}
	}
}