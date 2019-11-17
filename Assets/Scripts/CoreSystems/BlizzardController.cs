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
		[SerializeField] int maxBlizzardValue = 5;

		[Header("Damage")]
		[SerializeField, EnumFlags] TargetType typesToDamage = TargetType.Allies;
		[SerializeField, Range(1, 10)] int damage = 1;

		[Space]
		[Header("Events")]
		public UnityEvent onExecute = null;
		public UnityEvent onReset = null;

		[Space]
		[Header("Test")]
		[SerializeField] KeyCode testKey = KeyCode.Asterisk;

		UnitRegistry ur = null;

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
			if (Input.GetKeyDown(testKey))
				Tick();
		}

		public void Tick()
		{
			blizzard.value++;
			if (blizzard.value > maxBlizzardValue)
			{
				Execute();
				Reset();
			}
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

		public void Reset()
		{
			blizzard.value = 0;
			onReset.Invoke();
		}
		#endregion
	}
}