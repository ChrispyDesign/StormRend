/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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