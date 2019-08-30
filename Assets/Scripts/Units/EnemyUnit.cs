using System.Collections;
using System.Collections.Generic;
using StormRend.Defunct;
using UnityEngine;

namespace StormRend
{
	public class EnemyUnit : Unit
	{
		public override void OnSelect()
		{
			Dijkstra.Instance.FindValidMoves(GetTile(), GetMoveRange(), typeof(PlayerUnit));

			base.OnSelect();
			GameManager.singleton.GetPlayerController().SetCurrentPlayer(null);
		}
	}
}