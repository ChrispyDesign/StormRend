using System.Collections.Generic;
using System.Linq;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Effects
{
	public sealed class AuraEffect : PassiveEffect
	{
		//Inspector
		[SerializeField] GameObject FX = null;

		//Members
		List<GameObject> auraPool = new List<GameObject>();

		public override void Initiate(Ability ability, Unit owner)
		{
			if (!FX)
			{
				Debug.LogWarning("Load FX prefab for this effect!");
				return;
			}

			//Find max amount of tiles this ability would take
			var targetTileCount = (from t in ability.castArea
						   where t == true
						   select t).Count();

			//Instantiate enough aura particles for this ability and save
			for (int i = 0; i < targetTileCount; ++i)
			{
				//Create
				var go = Instantiate(FX, owner.transform.position, owner.transform.rotation);
				
				//Hide
				go.SetActive(false);

				//Record
				auraPool.Add(go);
			}
		}

		/// <summary>
		/// Reposition the aura fxs
		/// </summary>
		public override bool OnUnitMoved(Ability ability, Unit owner, Unit moved)
		{
			var targetTiles = ability.GetTargetTiles(owner.currentTile);
			foreach (var t in targetTiles)
			{
				//Unhide and place a FX here from the pool

			}

			//Hide remaining fxs
			
			return false; 	//temp
		}
	}
}