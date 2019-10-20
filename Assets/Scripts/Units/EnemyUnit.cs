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
			Dijkstra.Instance.FindValidMoves(GetTile(), GetMoveRange(), typeof(PlayerUnit));

			base.OnSelect();
			GameManager.singleton.GetPlayerController().SetCurrentPlayer(null);
		}

		//This doesn't need to be overriden
		public override void Die()
		{
			base.Die();
			GameManager.singleton.enemyCount--;
			GameManager.singleton.CheckEndCondition();
		}
	}
}