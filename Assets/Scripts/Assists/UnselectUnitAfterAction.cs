using StormRend.Abilities;
using StormRend.Systems;
using UnityEngine;

namespace StormRend.Assists
{
    /// <summary>
    /// Auto unselected the current unit once it performed an ability
    /// </summary>
    public class UnselectUnitAfterAction : MonoBehaviour
	{
        //Members
		UserInputHandler uih = null;

        void Awake()
		{
			uih = UserInputHandler.current;

            //Register events
            uih.onAbilityPerformed.AddListener(OnAbilityPerformed); 
		}
        void OnDestroy() => uih.onAbilityPerformed.RemoveListener(OnAbilityPerformed);

        void OnAbilityPerformed(Ability a)
        {
            //Exit if the unit can still take action
            if (uih.selectedAnimateUnit.canAct) return;

            //Otherwise unselect the unit
            uih.ClearSelectedUnit();
        }
	}
}