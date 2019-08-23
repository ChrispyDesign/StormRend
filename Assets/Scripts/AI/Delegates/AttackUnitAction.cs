using System;
using System.Collections.Generic;
using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using BhaVE.Variables;
using UnityEngine;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// Attacks the units in the input list (hopefully only one left)
    /// </summary>
    [CreateAssetMenu(menuName = "StormRend/Delegates/Actions/AttackUnit", fileName = "AttackUnit")]
    public class AttackUnitAction : BhaveAction
    {
        [SerializeField] BhaveUnitList targets;

        Unit unit;

        public override void Initiate(BhaveAgent agent)
        {
            unit = agent.GetComponent<Unit>();
        }

        public override NodeState Execute(BhaveAgent agent)
        {
            Debug.Log("AttackUnitAction");

			//Get abiltiies, get effects and then 
            Ability passive = null;
            Ability[] first = null, second = null;
            unit.GetAbilities(ref passive, ref first, ref second);
			List<Effect> effects = first[0].GetEffects();

            unit.SetSelectedAbility(first[0]);


			//foreach (var t in targets.value)
			//{
			Animator anim = agent.GetComponentInChildren<Animator>();
			anim.SetInteger("AttackAnim", 1);
				if (unit is EnemyUnit)
				{
					//Should be encapsulted
					foreach(Effect effect in effects)
					{
						Tile coord = Grid.CoordToTile(targets.value[0].coords);
						effect.PerformEffect(coord, unit);
						
					}
				}
				else if (unit is PlayerUnit)
				{
					throw new NotImplementedException();
				}
            //}
            return NodeState.Success;
        }
    }
}
