﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public override void OnSelect()
    {
        Dijkstra.Instance.FindValidMoves(GetCurrentNode(), GetMove(), typeof(PlayerUnit));
        
        base.OnSelect();
        PlayerController.SetCurrentPlayer(null);
    }
}
