﻿using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using BhaVE.Variables;
using UnityEngine;

namespace StormRend.Bhaviours
{
	/// <summary>
	/// Attacks the units in the list (hopefully only one left)
	/// </summary>
	// [CreateAssetMenu(menuName = "StormRend/Delegates/Actions/AttackUnit", fileName = "AttackUnit")]
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


			return NodeState.Success;
        }
	}
}
