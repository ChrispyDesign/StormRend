using UnityEngine;
using UnityEngine.Events;
using StormRend.Utility.Attributes;
using StormRend.Units;
using pokoro.BhaVE.Core.Variables;
using StormRend.Enums;
using System.Collections.Generic;
using System.Collections;

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
        [SerializeField, EnumFlags] TargetType typesToDamage = TargetType.Allies;
        [SerializeField, Range(1, 10)] int damage = 1;

        [Space]
        [Header("Events")]
        public UnityEvent onExecute = null;
        public UnityEvent onReset = null;

        [Space]
        [Header("Test")]
        [SerializeField] KeyCode testKey = KeyCode.Tab;

        UnitRegistry ur = null;

        #region Core
        void Start()
        {
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
        internal void Execute()
        {
            onExecute.Invoke();

            var unitsToDamage = new List<Unit>();

            //ALLIES
            if ((typesToDamage & TargetType.Allies) == TargetType.Allies)
                unitsToDamage.AddRange(ur.GetUnitsByType<AllyUnit>());
            //ENEMIES
            if ((typesToDamage & TargetType.Enemies) == TargetType.Enemies)
                unitsToDamage.AddRange(ur.GetUnitsByType<EnemyUnit>());
            //CRYSTALS
            if ((typesToDamage & TargetType.Crystals) == TargetType.Crystals)
                unitsToDamage.AddRange(ur.GetUnitsByType<CrystalUnit>());
            //INANIMATES
            if ((typesToDamage & TargetType.InAnimates) == TargetType.InAnimates)
                unitsToDamage.AddRange(ur.GetUnitsByType<InAnimateUnit>());
            //ANIMATES
            if ((typesToDamage & TargetType.Animates) == TargetType.Animates)
                unitsToDamage.AddRange(ur.GetUnitsByType<AnimateUnit>());

            //Deal damage to selected units
            foreach (var u in unitsToDamage)
                u.TakeDamage(new HealthData(null, damage));
        }

        internal void Reset()
        {
            onReset.Invoke();
            blizzardVar.value = 0;
        }
        #endregion
    }
}