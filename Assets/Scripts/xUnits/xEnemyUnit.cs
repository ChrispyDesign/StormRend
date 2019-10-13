using StormRend.Defunct;

namespace StormRend
{
	public class xEnemyUnit : xUnit
	{
		public override void OnSelect()
		{
			xDijkstra.Instance.FindValidMoves(GetTile(), GetMoveRange(), typeof(xPlayerUnit));

			base.OnSelect();
			xGameManager.singleton.GetPlayerController().SetCurrentPlayer(null);
		}
	}
}