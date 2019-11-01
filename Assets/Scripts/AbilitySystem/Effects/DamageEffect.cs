using System.Collections.Generic;
using System.Linq;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public class DamageEffect : Effect
    {
        [SerializeField] int damage = 1;

		public override void Perform(Unit owner, Tile[] targetTiles)
        {
 			//Get and convert to lists where required
			Unit[] units = UnitRegistry.current.aliveUnits;
			List<Tile> tt = targetTiles.ToList();

			foreach (var u in units)
			{
				if (tt.Contains(u.currentTile))
				{
					var au = owner as AnimateUnit;

					//Face victim
					au.SnappedLookAt(u.transform.position);

                    //Damage units that are standing on target tiles
                    u.TakeDamage(new DamageData(owner, damage));
				}
			}
        }
    }
}
