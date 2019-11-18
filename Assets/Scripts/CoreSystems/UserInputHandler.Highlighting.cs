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
			ShowActionTiles();
		}
		public void OnUnhoverPreview()
		{
			//Must be in move mode
			if (mode != Mode.Move) return;

			//Redraw
			ClearAllTileHighlights();
			ShowMoveTiles();
		}

		void ShowMoveTiles()
		{
			//NOTE: Active unit's MOVE highlights should be refreshed:
			// - At the start of each turn
			// - After another unit has summoned something
			if (!selectedAnimateUnit.canMove) return;

			if (selectedAnimateUnit.possibleMoveTiles.Length <= 0)
				selectedAnimateUnit.CalculateMoveTiles();

			//Highlight
			foreach (var t in selectedAnimateUnit?.possibleMoveTiles)
				t.SetHighlight(moveHighlight);
		}

		void ShowActionTiles()
		{
			//NOTE: Active unit's ACTION highlights should be refreshed
			// - each time the selected ability is changed
			if (selectedAnimateUnit.possibleTargetTiles.Length <= 0) return;

			//Highlight
			foreach (var t in selectedAnimateUnit?.possibleTargetTiles)
				t.SetHighlight(actionHighlight);
		}

		void ShowTargetTile(Tile target)
		{
			if (targetTileStack.Count <= 0) return;
			target.SetHighlight(targetHighlight);
		}

		Tile ClearTargetTile(Tile target)
		{
			//it should be action highlight
			target.SetHighlight(actionHighlight);
			return target;
		}
		#endregion
	}
}