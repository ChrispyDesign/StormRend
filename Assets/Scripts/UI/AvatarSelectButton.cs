using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using StormRend.Units;
using StormRend.Utility.Events;
using UnityEngine.Events;
using StormRend.Systems;
using StormRend.Tags;
using System;
using StormRend.Utility.Attributes;
using StormRend.Enums;
using StormRend.Abilities;
using System.Linq;

namespace StormRend.UI
{
	[RequireComponent(typeof(Button))]
	public class AvatarSelectButton : MonoBehaviour     //, IPointerEnterHandler, IPointerExitHandler
	{

		//Inspector
		[Header("Settings")]
		[SerializeField] AllyType allyType = AllyType.Berserker;
		[SerializeField] Color greyOut = Color.grey;
		[SerializeField] Color blackOut = Color.black;

		[Header("Sprite References")]
		[SerializeField] Image[] avatarSprites = null;
		[SerializeField] List<Image> healthNodes = new List<Image>();

		//Members
		Button button = null;
		AnimateUnit unit = null;
		UserInputHandler userInputHandler = null;
		Color[] origColors = null;

		//Core
		void Awake()
		{
			button = GetComponent<Button>();

			userInputHandler = FindObjectOfType<UserInputHandler>();
			Debug.Assert(userInputHandler, string.Format("[{0}] {1} not found!", this.name, typeof(UserInputHandler).Name));

			AutoLocateUnit();
			RegisterEvents();
			SaveOriginalColors();
		}

		void SaveOriginalColors()
		{
			Debug.Assert(avatarSprites != null && avatarSprites.Length > 0, "No avatars sprites loaded in!");
			if (avatarSprites == null || avatarSprites.Length <= 0) return;
			var colors = from s in avatarSprites select s.color;
			origColors = colors.ToArray();
		}

		void RegisterEvents()
		{
			//Button
			button.onClick.AddListener(SelectUnit);

			//Health pips
			unit?.onHeal.AddListener(UpdateHealthGUI);
			unit?.onTakeDamage.AddListener(UpdateHealthGUI);
			unit?.onDeath.AddListener(UpdateHealthGUIRelay);

			//Active
			userInputHandler.onUnitSelected.AddListener(OnActedRelay);
			unit?.onActed.AddListener(OnActed);
			unit?.onBeginTurn.AddListener(OnActedRelay);

			//Death
			unit?.onDeath.AddListener(OnDeath);
		}

		void OnDestroy()
		{
			button.onClick.RemoveListener(SelectUnit);
			unit?.onHeal.RemoveListener(UpdateHealthGUI);
			unit?.onTakeDamage.RemoveListener(UpdateHealthGUI);
			unit?.onDeath.AddListener(UpdateHealthGUIRelay);
			unit?.onActed.RemoveListener(OnActed);
			unit?.onDeath.AddListener(OnDeath);
		}

		void AutoLocateUnit()
		{
			Type typeToFind = null;
			switch (allyType)
			{
				case AllyType.Berserker:
					typeToFind = typeof(BerserkerTag);
					break;
				case AllyType.Valkyrie:
					typeToFind = typeof(ValkyrieTag);
					break;
				case AllyType.Sage:
					typeToFind = typeof(SageTag);
					break;
			}
			var tag = FindObjectOfType(typeToFind) as Tag;
			unit = tag?.GetComponent<AnimateUnit>();

			if (!unit)
			{
				Debug.LogWarningFormat("[{0}] Unit not found! Shutting down...", this.name);
				transform.parent.gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// On click
		/// </summary>
		public void SelectUnit() => userInputHandler.SelectUnit(unit, true);

		public void UpdateHealthGUIRelay(Unit NULL) => UpdateHealthGUI(new HealthData());
		public void UpdateHealthGUI(HealthData NULL)
		{
			//Clear
			foreach (var n in healthNodes)
				n.fillAmount = 0f;
			//Fill
			for (int i = 0; i < unit.HP; i++)
				healthNodes[i].fillAmount = 1f;
		}

		//Hook up to AnimateUnit.onActed and .on
		public void OnActedRelay(Unit u) => OnActed(null);
		public void OnActedRelay() => OnActed(null);
		public void OnActed(Ability NULL)
		{
			if (unit.isDead) return;
			
			//Grey out if the unit cannot act anymore and cannot move anymore
			SetGreyOut(!unit.canMove && !unit.canAct);
		}

		public void OnDeath(Unit u)
		{
			//Black out the portraits
			if (u == unit) 	//extra check?
				SetBlackOut(u == unit);
		}

		/// <summary>
		/// Either set avatar sprites to greyed out or original colors depending on setting passed in
		/// </summary>
		public void SetGreyOut(bool active)
		{
			for (int i = 0; i < avatarSprites.Length; ++i)
				avatarSprites[i].color = active ? greyOut : origColors[i];
		}

		/// <summary>
		/// Either set avatar sprites to blacked out or original colors depending on setting passed in
		/// </summary>
		public void SetBlackOut(bool active)
		{
			for (int i = 0; i < avatarSprites.Length; ++i)
				avatarSprites[i].color = active ? blackOut : origColors[i];
		}
	}
}