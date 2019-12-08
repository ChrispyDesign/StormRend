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

namespace StormRend.Abilities.Effects
{
	public abstract class PassiveEffect : Effect
	{
		/// <returns>True if unit killed</returns>
		public virtual bool OnUnitKilled(Ability ability, Unit owner, Unit killed) { return false; }
		
		/// <returns>True if unit created</returns>
		public virtual bool OnUnitCreated(Ability ability, Unit owner, Unit created) { return false; }
		
		/// <returns>True if unit moved (moved is kinda a dud just so this function be passed in as a delegate)</returns>
		public virtual bool OnUnitMoved(Ability ability, Unit owner, Unit moved) { return false; }

		public sealed override void Perform(Ability ability, Unit owner, Tile[] targetTiles)
		{
			Debug.LogWarning("Cannot directly perform a passive effect. No action taken.");
		}
	}
}