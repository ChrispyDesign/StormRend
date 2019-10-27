using UnityEngine;
using UnityEngine.Events;
using StormRend.Utility.Attributes;
using StormRend.Units;
using pokoro.BhaVE.Core.Variables;
using StormRend.Enums;
using System.Collections.Generic;

namespace StormRend.Systems
{
	/// <summary>
	/// Applies blizzard to selected unit types
	/// </summary>
	public class BlizzardController : MonoBehaviour
    {
        //Inspector
        [SerializeField] BhaveInt blizzardVar = null;
        [SerializeField] int maxBlizzardValue = 5;

        [Header("Damage")]
        [SerializeField, EnumFlags] TargetMask targetUnits = TargetMask.Allies;
        [SerializeField, Range(1, 10)] int damage = 1;

        [Space]
        [Header("Events")]
        [SerializeField] UnityEvent OnExecute = null;
        [SerializeField] UnityEvent OnReset = null;

        UnitRegistry ur = null;

    #region Core
        void Start()
        {
            ur = UnitRegistry.current;
        }
        public void Tick()
        {
            blizzardVar++;
            if (blizzardVar > maxBlizzardValue)
            {
                Execute();
                Reset();
            }
        }
        internal void Execute()
        {
            OnExecute.Invoke();

            var unitsToDamage = new List<Unit>();

            //ALLIES
            if ((targetUnits & TargetMask.Allies) == TargetMask.Allies)
                unitsToDamage.AddRange(ur.GetUnitsByType<AllyUnit>());
            //ENEMIES
            if ((targetUnits & TargetMask.Enemies) == TargetMask.Enemies)
                unitsToDamage.AddRange(ur.GetUnitsByType<EnemyUnit>());
			//CRYSTALS
            if ((targetUnits & TargetMask.Crystals) == TargetMask.Crystals)
                unitsToDamage.AddRange(ur.GetUnitsByType<CrystalUnit>());
            //INANIMATES
            if ((targetUnits & TargetMask.InAnimates) == TargetMask.InAnimates)
                unitsToDamage.AddRange(ur.GetUnitsByType<InAnimateUnit>());
            //ANIMATES
            if ((targetUnits & TargetMask.Animates) == TargetMask.Animates)
                unitsToDamage.AddRange(ur.GetUnitsByType<AnimateUnit>());
			
			//Deal damage to selected units
			foreach (var u in unitsToDamage)
				u.TakeDamage(damage);
        }

        internal void Reset()
        {
            OnReset.Invoke();
            blizzardVar = 0;
        }
    #endregion
    }
}