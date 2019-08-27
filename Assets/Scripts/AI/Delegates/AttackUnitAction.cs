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

		Unit u;
		Animator anim;

		public override NodeState Execute(BhaveAgent agent)
		{
			//Make sure there are targets to attack
			if (targets.value.Count <= 0) return NodeState.Failure;

			//Get this agent's unit and animator (BAD)
			u = agent.GetComponent<Unit>();
			anim = u.GetComponentInChildren<Animator>();

			//Get abiltiies then attack effect
			Ability passive = null;
			Ability[] first = null;
			Ability[] second = null;
			u.GetAbilities(ref passive, ref first, ref second);
			List<Effect> effects = first[0].GetEffects();

			//Attack! (+animate) (BAD)
			u.SetSelectedAbility(first[0]);
			anim.SetInteger("AttackAnim", 1);

			if (u is EnemyUnit)
			{
				//Should be encapsulted
				foreach (Effect effect in effects)
				{
					Tile coord = Grid.CoordToTile(targets.value[0].coords);
					effect.PerformEffect(coord, u);

				}
			}
			else if (u is PlayerUnit)
			{
				throw new NotImplementedException();
			}

			return NodeState.Success;
		}
	}
}
