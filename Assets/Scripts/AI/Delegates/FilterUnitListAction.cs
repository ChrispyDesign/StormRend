using System.Linq;
using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using BhaVE.Variables;
using UnityEngine;

namespace StormRend.Bhaviours
{
	/// <summary>
	/// Filters out lists based on the filter method
	/// </summary>
	[CreateAssetMenu(menuName = "StormRend/Delegates/Actions/FilterUnitList", fileName = "FilterUnitList")]
    public class FilterUnitListAction : BhaveAction
    {
        public enum FilterMode { LowestHP, Closest }

        public FilterMode filterMethod;
        [SerializeField] BhaveUnitList targets;

        //Privates
        Unit unit;

        public override void Initiate(BhaveAgent agent)
        {
            unit = agent.GetComponent<Unit>();
        }

        public override NodeState Execute(BhaveAgent agent)
        {
            switch (filterMethod)
            {
                case FilterMode.Closest:
                {
					//Sort list based on distance from this unit
					targets.value = targets.value.
						OrderBy(x => Vector2Int.Distance(x.m_coordinates, unit.m_coordinates)).ToList();

					//Filter by closest
					targets.value = targets.value.
						Where(x => Vector2Int.Distance(x.m_coordinates, unit.m_coordinates).
							Equals(Vector2Int.Distance(targets.value[0].m_coordinates, unit.m_coordinates))).ToList(); 	//The first value in the list should be the closest unit
                }
				break;

                case FilterMode.LowestHP:
                {
					//Sort
					targets.value = targets.value.OrderBy(x => x.HP).ToList();

					//Filter
					targets.value = targets.value.Where(x => x.HP == targets.value[0].HP).ToList();
                }
				break;

				default: return NodeState.Failure;
            }
			return NodeState.Success;
        }

    }
}
