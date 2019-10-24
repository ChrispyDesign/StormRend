using StormRend.Defunct;
using StormRend.MapSystems.Tiles;
using StormRend.Units;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class RefreshEffect : Effect
    {
		// public enum RefreshType
		// {
		// 	AttackAgain,
		// 	MoveAgain
		// }
        // [SerializeField] RefreshType refreshType;
		// [HelpBox, SerializeField] string info = "wtf mate";

		public override bool Perform(Unit owner, Tile[] targetTiles)
        {
			var au = owner as AnimateUnit;
			au.SetActed(false);		//You should always be able to move again right?
            return true;
        }
    }
}