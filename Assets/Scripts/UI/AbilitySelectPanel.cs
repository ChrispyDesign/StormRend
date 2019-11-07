﻿using UnityEngine;
using System.Collections.Generic;
using StormRend.Variables;
using StormRend.Units;

namespace StormRend.UI
{
	public class AbilitySelectPanel : MonoBehaviour
	{
		[SerializeField] List<AbilityDetails> abilityDetails;

		private void Start()
		{
			for (int i = 0; i < abilityDetails.Count; i++)
			{
				abilityDetails[i].gameObject.SetActive(false);
			}
		}

		public void SelectedUnitOnChange(Unit unit)
		{
			AnimateUnit animateUnit = unit as AnimateUnit;

			for (int i = 0; i < animateUnit.abilities.Length; i++)
			{
				abilityDetails[i].gameObject.SetActive(true);
				abilityDetails[i].SetIcon(animateUnit.abilities[i].icon);
				abilityDetails[i].SetAbility(animateUnit.abilities[i]);
			}
		}
	}
}