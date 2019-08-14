using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Nodes;
using BhaVE.Variables;
using UnityEngine;

namespace StormRend.Bhaviours
{
    /// <summary>
    /// </summary>
    public class FilterUnitListAction : BhaveAction
    {
        public enum FilterMethod { LowestHP, Closest }
        public FilterMethod filterMethod;
        [SerializeField] BhaveUnitList targetList;

        //Privates
        private Unit thisUnit;

        public override void Initiate(BhaveAgent agent)
        {
            thisUnit = agent.GetComponent<Unit>();
        }


        public override NodeState Execute(BhaveAgent agent)
        {
            switch (filterMethod)
            {
                case FilterMethod.Closest:
                {
                    //Get this unit's coordinates

                    //Compare units in target list

                    // var closestList = targetList.value.

                }
                break;
                case FilterMethod.LowestHP:
                {

                }
                break;
            }
            throw new System.NotImplementedException();
        }

    }
}
