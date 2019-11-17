using UnityEngine;
using UnityEngine.Events;
using StormRend.Utility.Attributes;
using StormRend.Units;
using pokoro.BhaVE.Core.Variables;
using StormRend.Enums;
using System.Collections.Generic;
using System.Collections;
using StormRend.Abilities;

namespace StormRend.Systems
{
    /// <summary>
    /// Applies blizzard to selected unit types
    /// </summary>
    public class BlizzardController : MonoBehaviour
    {
        //Inspector
        [SerializeField] BhaveInt blizzardVar = null;
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
        //CRUNCH!
        void Awake()
        {
            blizzardVar.value = 0;
            ur = UnitRegistry.current;
        }
        void Update()
        {
            if (Input.GetKeyDown(testKey))
                Execute();
        }
        public void Tick()
        {
            blizzardVar.value++;
            if (blizzardVar.value > maxBlizzardValue)
            {
                Execute();
                Reset();
            }
        }
        public void Execute()
        {
            onExecute.Invoke();

            var unitsToDamage = new List<Unit>();

            //ALLIES
            if ((typesToDamage & TargetType.Allies) == TargetType.Allies)
                unitsToDamage.AddRange(ur.GetAliveUnitsByType<AllyUnit>());
            //ENEMIES
            if ((typesToDamage & TargetType.Enemies) == TargetType.Enemies)
                unitsToDamage.AddRange(ur.GetAliveUnitsByType<EnemyUnit>());
            //CRYSTALS
            if ((typesToDamage & TargetType.Crystals) == TargetType.Crystals)
                unitsToDamage.AddRange(ur.GetAliveUnitsByType<CrystalUnit>());
            //INANIMATES
            if ((typesToDamage & TargetType.InAnimates) == TargetType.InAnimates)
                unitsToDamage.AddRange(ur.GetAliveUnitsByType<InAnimateUnit>());
            //ANIMATES
            if ((typesToDamage & TargetType.Animates) == TargetType.Animates)
                unitsToDamage.AddRange(ur.GetAliveUnitsByType<AnimateUnit>());

            //Deal damage to selected units
            foreach (var u in unitsToDamage)
            {
                var au = u as AnimateUnit;

                //Damage
                u.TakeDamage(new HealthData(null, damage));

                //Immobilse
                immobilise.Perform(null, u);
            }

            Reset();
        }

        public void Reset()
        {
            onReset.Invoke();
            blizzardVar.value = 0;
        }
        #endregion
    }
}