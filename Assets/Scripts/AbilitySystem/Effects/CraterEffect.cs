/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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