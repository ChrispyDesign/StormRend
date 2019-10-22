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
        [SerializeField, EnumFlags] TargetUnitMask targetUnits = TargetUnitMask.Allies;
        [SerializeField, Range(1, 10)] int damage = 1;

        [Space]
        [Header("Events")]
        [SerializeField] UnityEvent OnExecute = null;
        [SerializeField] UnityEvent OnReset = null;

        UnitRegistry ur = null;

    #region Core
        void Awake()
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
            if ((targetUnits & TargetUnitMask.Allies) == TargetUnitMask.Allies)
                unitsToDamage.AddRange(ur.GetUnits<AllyUnit>());

            //ENEMIES
            if ((targetUnits & TargetUnitMask.Enemies) == TargetUnitMask.Enemies)
                unitsToDamage.AddRange(ur.GetUnits<EnemyUnit>());

            //CRYSTALS
            if ((targetUnits & TargetUnitMask.Crystals) == TargetUnitMask.Crystals)
                unitsToDamage.AddRange(ur.GetUnits<CrystalUnit>());

            //INANIMATES
            if ((targetUnits & TargetUnitMask.InAnimates) == TargetUnitMask.InAnimates)
                unitsToDamage.AddRange(ur.GetUnits<InAnimateUnit>());

            //ANIMATES
            if ((targetUnits & TargetUnitMask.Animates) == TargetUnitMask.Animates)
                unitsToDamage.AddRange(ur.GetUnits<AnimateUnit>());

            DealDamageToUnits(unitsToDamage.ToArray());

            void DealDamageToUnits(Unit[] units)
            {
                foreach (var u in units)
                {
                    u.TakeDamage(damage);
                }
            }
        }

        internal void Reset()
        {
            OnReset.Invoke();
            blizzardVar = 0;
        }
    #endregion
    }
}