using pokoro.BhaVE.Core;
using pokoro.BhaVE.Core.Delegates;
using pokoro.BhaVE.Core.Enums;
using StormRend.Abilities;
using StormRend.Units;
using StormRend.Variables;
using UnityEngine;

namespace StormRend.Bhaviours
{
	/// <summary>
	/// Attacks the units in the input list (hopefully only one left)
	/// </summary>
	[CreateAssetMenu(menuName = "StormRend/AI/AttackAction", fileName = "AttackAction")]
    public sealed class AttackAction : BhaveAction
    {
        [SerializeField] UnitListVar targets = null;
		[SerializeField] Ability attackAbility = null;

        AnimateUnit au = null;

		public override void Awaken(BhaveAgent agent)
		{
			au = agent.GetComponent<AnimateUnit>();		//TODO this might not work
		}

        public override NodeState Execute(BhaveAgent agent)
        {
        	//Make sure there are targets to attack
        	if (targets.value.Count <= 0) return NodeState.Failure;

			//Attack target
			if (au.abilities[0] != null)
			{
				au.FilteredAct(au.abilities[0], targets.value.ToArray());
        		return NodeState.Success;
			}
			else
			{
				Debug.LogWarning("No ability found!");
				return NodeState.Failure;
			}
		}
    }
}
