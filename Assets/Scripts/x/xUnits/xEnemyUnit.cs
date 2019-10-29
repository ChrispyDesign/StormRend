﻿using StormRend.Defunct;

namespace StormRend.Defunct
{
	public class xEnemyUnit : xUnit
	{
		public override void OnSelect()
		{
			xDijkstra.Instance.FindValidMoves(GetTile(), GetMoveRange(), typeof(xPlayerUnit));

			base.OnSelect();
			xGameManager.current.GetPlayerController().SetCurrentPlayer(null);
		}
	}
}