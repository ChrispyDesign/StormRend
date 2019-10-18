using System;
using System.Collections.Generic;
using pokoro.BhaVE.Core;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core.Enums;
using StormRend.Abilities;
using StormRend.Abilities.Effects;
using StormRend.Defunct;
using StormRend.Variables;
using UnityEngine;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// Attacks the units in the input list (hopefully only one left)
    /// </summary>
    [CreateAssetMenu(menuName = "StormRend/Delegates/Actions/AttackUnit", fileName = "AttackUnit")]
    public class AttackUnitAction : BhaveAction
    {
        // [SerializeField] UnitListVar targets;

        // xUnit u;
        // Animator anim;

        // public override NodeState Execute(BhaveAgent agent)
        // {
        // 	//Make sure there are targets to attack
        // 	if (targets.value.Count <= 0) return NodeState.Failure;

        // 	//Get this agent's unit and animator (BAD)
        // 	u = agent.GetComponent<xUnit>();
        // 	anim = u.GetComponentInChildren<Animator>();	//VERY BAD

        // 	//Get abiltiies then attack effect
        // 	xAbility passive = null;
        // 	xAbility[] first = null;
        // 	xAbility[] second = null;
        // 	u.GetAbilities(ref passive, ref first, ref second);
        // 	List<xEffect> effects = first[0].GetEffects();

        // 	//Attack!
        // 	u.SetSelectedAbility(first[0]);
        // 	anim.SetInteger("AttackAnim", 1);	//THIS IS BAD!!!

        // 	if (u is xEnemyUnit)
        // 	{
        // 		//SHOULD BE ENCAPSULATED/SIMPLE METHOD CALL
        // 		foreach (xEffect effect in effects)
        // 		{
        // 			xTile targetTile = xGrid.CoordToTile(targets.value[0].coords);
        // 			effect.PerformEffect(targetTile, u);
        // 		}
        // 	}
        // 	else if (u is xPlayerUnit)
        // 	{
        // 		throw new NotImplementedException("PlayerUnit AI not implemented!");
        // 	}

        // 	return NodeState.Success;
        public override NodeState Execute(BhaveAgent agent)
        {
            throw new NotImplementedException();
        }
    }
}
