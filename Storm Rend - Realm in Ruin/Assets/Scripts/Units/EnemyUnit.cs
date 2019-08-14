using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;

public class EnemyUnit : Unit
{
    public override void OnSelect()
    {
        Dijkstra.Instance.FindValidMoves(GetCurrentNode(), GetMove(), typeof(PlayerUnit));
        
        base.OnSelect();
        GameManager.GetInstance().GetPlayerController().SetCurrentPlayer(null);
	}

	public override void Die()
	{
		base.Die();
		GameManager.GetInstance().m_enemyCount--;
		GameManager.GetInstance().CheckEndCondition();
	}
}
