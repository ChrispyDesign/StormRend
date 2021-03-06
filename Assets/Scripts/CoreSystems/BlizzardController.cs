/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;
using UnityEngine.Events;
using StormRend.Utility.Attributes;
using StormRend.Units;
using pokoro.BhaVE.Core.Variables;
using StormRend.Enums;
using System.Collections.Generic;
using StormRend.Abilities;

namespace StormRend.Systems
{
	/// <summary>
	/// Applies blizzard to selected unit types
	/// </summary>
	public class BlizzardController : MonoBehaviour
	{
		//Inspector
		[SerializeField] BhaveInt blizzard = null;
		[SerializeField] Ability immobilise = null;
		[SerializeField] int blizzardTriggerValue = 5;

		[Header("Damage")]
		[SerializeField, EnumFlags] TargetType typesToDamage = TargetType.Allies;
		[SerializeField, Range(1, 10)] int damage = 1;

		[Space]
		[Header("Events")]
		public UnityEvent onExecute = null;

		//Members
		UnitRegistry ur = null;

		//Debug
		[Space]
		[SerializeField] bool debug = false;
		[SerializeField] KeyCode tickKey = KeyCode.Equals;

		#region Core
		void Awake() => ur = UnitRegistry.current;
		void Start()
		{
			Debug.Assert(blizzard, "No blizzard SOV loaded in!");
			Debug.Assert(blizzard, "No blizzard immobilise ability loaded in!");
			blizzard.value = 0;  //Reset on start
			
		}

		void Update()
		{
			if (!debug) return;

			if (Input.GetKeyDown(tickKey))
				Tick();
		}

		public void Tick()
		{
			if (blizzard.value == blizzardTriggerValue)
				Execute();

			blizzard.value++;
		}

		/// <summary>
		/// Damages and immobilises specified units
		/// </summary>
		public void Execute()
		{
			bool targetAnimates = false; bool targetInanimates = false;

			onExecute.Invoke();

			var unitsToDamage = new List<Unit>();

			//ANIMATES
			if ((typesToDamage & TargetType.Animates) == TargetType.Animates)
				targetAnimates = true;
			//INANIMATES
			if ((typesToDamage & TargetType.InAnimates) == TargetType.InAnimates)
				targetInanimates = true;
			//ALLIES
			if (targetAnimates || (typesToDamage & TargetType.Allies) == TargetType.Allies)
				unitsToDamage.AddRange(ur.GetAliveUnitsByType<AllyUnit>());
			//ENEMIES
			if (targetAnimates || (typesToDamage & TargetType.Enemies) == TargetType.Enemies)
				unitsToDamage.AddRange(ur.GetAliveUnitsByType<EnemyUnit>());
			//CRYSTALS
			if (targetInanimates || (typesToDamage & TargetType.Crystals) == TargetType.Crystals)
				unitsToDamage.AddRange(ur.GetAliveUnitsByType<CrystalUnit>());

			//Deal damage to selected units
			foreach (var u in unitsToDamage)
				u.TakeDamage(new HealthData(null, damage));

			//Immobilise
			immobilise?.Perform(null, unitsToDamage.ToArray());
		}
		#endregion
	}
}