/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;
using System.Collections.Generic;
using StormRend.Units;

namespace StormRend.UI
{
	public class AbilitySelectPanel : MonoBehaviour
	{
		[SerializeField] List<AbilityDetails> abilityDetails = new List<AbilityDetails>();

		private void Start()
		{
			for (int i = 0; i < abilityDetails.Count; i++)
			{
				abilityDetails[i].gameObject.SetActive(false);
			}
		}

		public void SelectedUnitOnChange(Unit u)
		{
			var au = u as AnimateUnit;

			if (au)
			{
				for (int i = 0; i < 6; i++)     //This doesn't account for the Sage's passive ability. Anything above a 7
				{
					abilityDetails[i].SetAbility(au.abilities[i]);
					abilityDetails[i].gameObject.SetActive(true);
				}
			}
			else
			{
				for (int i = 0; i < 6; i++)
				{
					abilityDetails[i].SetAbility(null);
					abilityDetails[i].gameObject.SetActive(false);
				}
			}
		}
		public void ClearSelectedUnit() => SelectedUnitOnChange(null);
	}
}