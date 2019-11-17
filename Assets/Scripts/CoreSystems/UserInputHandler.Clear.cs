using pokoro.Patterns.Generic;
using StormRend.MapSystems;

namespace StormRend.Systems
{
	public partial class UserInputHandler : Singleton<UserInputHandler>
	{
		#region Clears
		//Deselects the unit
		void ClearSelectedUnit()
		{
			if (!isUnitSelected) return;    //A unit should be selected

			onUnitCleared.Invoke();

			//Clear tile highlights and ghost
			ClearSelectedUnitTileHighlights();
			selectedAnimateUnit.ClearGhost();

			//Clear
			selectedUnit = null;
		}

		void ClearSelectedAbility(bool redrawMoveTiles = true)
		{
			if (!isUnitSelected) return;    //A unit should be selected

			onAbilityCleared.Invoke();

			//Clear
			selectedAbility = null;

			//Clear tile highlights
			if (isUnitSelected)
				ClearSelectedUnitTileHighlights();

			//Redraw move highlights
			if (redrawMoveTiles)
				ShowMoveTiles();
		}

		void ClearSelectedUnitTileHighlights()
		{
			if (!isUnitSelected) return;    //A unit should be selected

			//Clear move highlights
			if (selectedAnimateUnit.possibleMoveTiles != null)
				foreach (var t in selectedAnimateUnit.possibleMoveTiles)
					t.ClearColor();

			//Clear target highlights
			if (selectedAnimateUnit.possibleTargetTiles != null)
				foreach (var t in selectedAnimateUnit.possibleTargetTiles)
					t.ClearColor();
		}

		//Trying to avoid the accidental unhover glitch but still doesn't solve it
		void ClearAllTileHighlights()
		{
			foreach (var t in Map.current.tiles)
			{
				t.ClearColor();
			}
		}
		#endregion
	}
}