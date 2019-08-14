﻿using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;

namespace StormRend
{
	public class EnemyUnit : Unit
	{

		public override void OnSelect()
		{
			Dijkstra.Instance.FindValidMoves(GetCurrentNode(), GetMove(), typeof(PlayerUnit));

			base.OnSelect();
			GameManager.singleton.GetPlayerController().SetCurrentPlayer(null);
		}

		//This doesn't need to be overriden
		public override void Die()
		{
			base.Die();
			GameManager.singleton.m_enemyCount--;
			GameManager.singleton.CheckEndCondition();
		}
	}
}