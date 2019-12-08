/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using pokoro.Patterns.Generic;
using StormRend.Abilities;
using StormRend.MapSystems.Tiles;

namespace StormRend.Systems
{
	public partial class UserInputHandler : Singleton<UserInputHandler>
	{
		#region Tile Highlighting
		//Show a preview of target tiles 
		public void OnHoverPreview(Ability a)
		{
			//Has to be in Move mode
			if (mode != Mode.Move) return;

			//Has to be able to act
			if (!selectedAnimateUnit.canAct) return;

			selectedAnimateUnit.CalculateTargetTiles(a);
			selectedAnimateUnit.ClearGhost();
			ShowActionTiles(a);
		}
		public void OnUnhoverPreview()
		{
			//Must be in move mode
			if (mode != Mode.Move) return;

			//Redraw
			ClearAllTileHighlights();
			ShowMoveTiles();
		}

		/// <summary>
		/// Show selected unit's current move tiles
		/// NOTE Unit's move tiles should be refreshed:
		/// - At the beginning of the unit's turn
		/// - When a unit is summoned or spawned in
		/// - When a unit is killed
		/// </summary>
		void ShowMoveTiles()
		{
			if (selectedAnimateUnit.canMove)
			{
				if (selectedAnimateUnit.possibleMoveTiles.Count <= 0)
					selectedAnimateUnit.CalculateMoveTiles();

				foreach (var t in selectedAnimateUnit?.possibleMoveTiles)
				{
					//Ignore the starting tile tile
					if (t != selectedAnimateUnit.startTile)
						t.SetHighlight(moveHighlight);
				}
			}

			//Starting tile
			selectedAnimateUnit.startTile.SetHighlight(startHighlight);
		}

		void ShowActionTiles(Ability a)
		{
			//NOTE: Active unit's ACTION highlights should be refreshed
			// - each time the selected ability is changed
			if (selectedAnimateUnit.possibleTargetTiles.Count <= 0) return;

			//Highlight
			foreach (var t in selectedAnimateUnit?.possibleTargetTiles)
			{
				//If targetable
				if (a.IsAcceptableTileType(selectedAnimateUnit, t))	
					t.SetHighlight(targetableHighlight);
				//Show action range
				else
					t.SetHighlight(untargetableHighlight);
			}
		}

		void ShowTargetTile(Tile target)
		{
			if (targetTileStack.Count <= 0) return;
			target.SetHighlight(targetHighlight);
		}

		Tile ClearTargetTile(Tile target)
		{
			//it should be action highlight
			target.SetHighlight(targetableHighlight);
			return target;
		}
		#endregion
	}
}