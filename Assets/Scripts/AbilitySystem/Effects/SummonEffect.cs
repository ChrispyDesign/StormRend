/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;
using StormRend.MapSystems.Tiles;
using StormRend.Units;

namespace StormRend.Abilities.Effects
{
	public class SummonEffect : Effect
    {
        [SerializeField] GameObject summon = null;

		public override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
        {
			//Summon at each target tile
			foreach (var t in targetTiles)
			{
				var inanimate = Instantiate(summon, t.gameObject.transform.position, Quaternion.identity, null).GetComponent<InAnimateUnit>();
				UnitRegistry.current.RegisterUnitCreation(inanimate);
			}
        }
    }
}