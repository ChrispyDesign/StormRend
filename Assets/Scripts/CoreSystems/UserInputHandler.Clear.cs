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
					t.SetHighlight(clearHighlight);

			//Clear target highlights
			if (selectedAnimateUnit.possibleTargetTiles != null)
				foreach (var t in selectedAnimateUnit.possibleTargetTiles)
					t.SetHighlight(clearHighlight);
		}

		//Trying to avoid the accidental unhover glitch but still doesn't solve it
		void ClearAllTileHighlights()
		{
			foreach (var t in Map.current.tiles)
			{
				t.SetHighlight(clearHighlight);
			}
		}
		#endregion
	}
}