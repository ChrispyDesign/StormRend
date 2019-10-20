using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;

namespace StormRend
{
	public class EnemyUnit : Unit
	{
		public override void OnSelect()
		{
			Dijkstra.Instance.GetValidMoves(GetTile(), GetMoveRange(), typeof(PlayerUnit));

			base.OnSelect();
			GameManager.singleton.GetPlayerController().SetCurrentPlayer(null);
		}
	}
}