using UnityEngine;
using StormRend.MapSystems.Tiles;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
	public class SummonEffect : Effect
    {
        [SerializeField] GameObject summon = null;

		public override void Perform(Unit owner, Tile[] targetTiles)
        {
			//Summon at each target tile
			foreach (var t in targetTiles)
			{
				var inanimate = Instantiate(summon, t.gameObject.transform.position, Quaternion.identity, null).GetComponent<InAnimateUnit>();
				UnitRegistry.current.RegisterUnit(inanimate);
			}
        }
    }
}