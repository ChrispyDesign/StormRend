﻿using System;
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
		CameraMove camMover;

		public override void Awaken(BhaveAgent agent)
		{
			camMover = FindObjectOfType<Camera>().GetComponent<CameraMove>();
		}

		public override NodeState Execute(BhaveAgent agent)
		{
			//Make sure there are targets to attack
			if (targets.value.Count <= 0) return NodeState.Failure;

			//Get this agent's unit and animator (BAD)
			u = agent.GetComponent<Unit>();
			anim = u.GetComponentInChildren<Animator>();	//VERY BAD

			//Get abiltiies then attack effect
			Ability passive = null;
			Ability[] first = null;
			Ability[] second = null;
			u.GetAbilities(ref passive, ref first, ref second);
			List<Effect> effects = first[0].GetEffects();

			//TODO TEMPORARY: Play animations
			u.SetSelectedAbility(first[0]);
			anim.SetInteger("AttackAnim", 1);	//THIS IS BAD!!!

			if (u is EnemyUnit)
			{
				//SHOULD BE ENCAPSULATED/SIMPLE METHOD CALL
				foreach (Effect effect in effects)
				{
					Tile targetTile = Grid.CoordToTile(targets.value[0].coords);
					effect.PerformEffect(targetTile, u);
				}

				//Focus camera on activity
				camMover.MoveTo(u.transform.position, 1f);
			}
			else if (u is PlayerUnit)
			{
				throw new NotImplementedException("PlayerUnit AI not implemented!");
			}

			return NodeState.Success;
		}
	}
}
