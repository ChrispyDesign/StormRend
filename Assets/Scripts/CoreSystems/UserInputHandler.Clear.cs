/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using pokoro.Patterns.Generic;
using StormRend.MapSystems;

namespace StormRend.Systems
{
	public partial class UserInputHandler : Singleton<UserInputHandler>
	{
		#region Clears
		//Deselects the unit
		public void ClearSelectedUnit()
		{
			if (!isUnitSelected) return;    //A unit should be selected

			//Clear tile highlights and ghost
			ClearAllTileHighlights();
			selectedAnimateUnit.ClearGhost();

			//Clear
			selectedUnit = null;

			//Events
			onUnitCleared.Invoke();
		}

		public void ClearSelectedAbility(bool redrawMoveTiles = true)
		{
			if (!isUnitSelected) return;    //A unit should be selected

			//Clear
			selectedAbility = null;

			//Clear tile highlights
			if (isUnitSelected)
				ClearSelectedUnitTileHighlights();

			//Redraw move highlights
			if (redrawMoveTiles)
				ShowMoveTiles();

			//Events
			onAbilityCleared.Invoke();
		}

		void ClearSelectedUnitTileHighlights()
		{
			if (!isUnitSelected) return;    //A unit should be selected

			//Clear move highlights
			if (selectedAnimateUnit.possibleMoveTiles != null)
				foreach (var t in selectedAnimateUnit.possibleMoveTiles)
					t.ClearHighlight();
					// t.SetHighlight(clearHighlight);

			//Clear target highlights
			if (selectedAnimateUnit.possibleTargetTiles != null)
				foreach (var t in selectedAnimateUnit.possibleTargetTiles)
					t.ClearHighlight();
					// t.SetHighlight(clearHighlight);
		}

		//Trying to avoid the accidental unhover glitch but still doesn't solve it
		void  ClearAllTileHighlights()
		{
			foreach (var t in Map.current.tiles)
				t.ClearHighlight();
		}
		#endregion
	}
}