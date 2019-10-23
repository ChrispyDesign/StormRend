﻿using System.Collections;
using System.Collections.Generic;
using StormRend;
using StormRend.Abilities;
using StormRend.Defunct;
using StormRend.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.UI
{
	public class xUIAbilityInfo : MonoBehaviour
	{
		[SerializeField] private xAbility m_ability;

		#region GettersAndSetters
		public xAbility GetAbility() { return m_ability; }
		public void SetAbility(xAbility _ability) { m_ability = _ability; }
		#endregion

		xUIAbilitySelector selector;
		GameObject infoPanel;
		xUnit allyUnit;

		void Awake()
		{
			selector = GetComponentInParent<xUIAbilitySelector>();
		}

		void Start()
		{
			Debug.Assert(selector, "No selector found!");
			infoPanel = selector.GetInfoPanel();
		}

		public void HoverAbility()
		{
			allyUnit = xGameManager.current.GetPlayerController().GetCurrentPlayer();
			if (allyUnit != null)
			{
				xGameManager.current.GetPlayerController().SetCurrentMode(SelectMode.Attack);
				selector.SetInfoPanelData();

				if (allyUnit.GetAttackTiles() != null &&
					allyUnit.GetAttackTiles().Count > 0)
					allyUnit.UnShowAttackTiles();

				m_ability.GetSelectableTiles(ref allyUnit);
				allyUnit.OnDeselect();
				allyUnit.ShowAttackTiles();
			}
			infoPanel.SetActive(true);
		}

		/// <summary>
		///
		/// </summary>
		public void UnhoverAbility()
		{
			bool isLockedAbility = xGameManager.current.GetPlayerController().GetIsAbilityLocked();

			if (!isLockedAbility)
				xGameManager.current.GetPlayerController().SetCurrentMode(SelectMode.Move);

			if (allyUnit != null)
			{
				allyUnit.UnShowAttackTiles();
			}
			infoPanel.SetActive(false);

			if (isLockedAbility)
			{
				xAbility ability = xGameManager.current.GetPlayerController().GetCurrentPlayer().GetSelectedAbility();
				xUnit player = xGameManager.current.GetPlayerController().GetCurrentPlayer() as xUnit;
				if (!player)
				{
					ability.GetSelectableTiles(ref player);
				}
				player.ShowAttackTiles();
			}
		}

		public void OnClickAbility()
		{
			Button button = this.gameObject.GetComponent<Button>();
			if (allyUnit != null && !allyUnit.GetHasAttacked() && button.interactable)
			{
				xGameManager.current.GetPlayerController().SetCurrentMode(SelectMode.Attack);
				allyUnit.SetSelectedAbility(m_ability);
				xGameManager.current.GetPlayerController().SetIsAbilityLocked(true); ;
				allyUnit.ShowAttackTiles();
			}
			infoPanel.SetActive(false);
		}
	}
}