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
				if (selectedAnimateUnit.possibleMoveTiles.Length <= 0)
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