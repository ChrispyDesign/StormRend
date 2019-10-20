using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using BhaVE.Variables;
using UnityEngine;

namespace StormRend.Bhaviours
{
	[CreateAssetMenu(menuName = "StormRend/Delegates/Actions/FilterUnitsByProvoke", fileName = "FilterUnitsByProvoke")]

	public class FilterUnitsByProvokeAction : BhaveAction
	{
		[SerializeField] BhaveUnitList targets;

		public override NodeState Execute(BhaveAgent agent)
		{
			foreach (var u in targets.value)
			{
				if (u.isProvoking)
				{
					targets.value.RemoveAll(x => !x.isProvoking);
					return NodeState.Success;	//Successfully found a provoking unit. All other units filtered out.
				}
			}
			return NodeState.Pending;	//Not provoked units found. Unit list not filtered. Continue as usual
		}
	}
}