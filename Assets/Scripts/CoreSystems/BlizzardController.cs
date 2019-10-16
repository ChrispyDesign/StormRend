using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using StormRend.Utility.Attributes;
using StormRend.Defunct;
using StormRend.Units;
using pokoro.BhaVE.Core.Variables;

namespace StormRend.Systems
{
    /// <summary>
    /// Applies blizzard to selected unit types
    /// </summary>
    public class BlizzardController : MonoBehaviour
    {
        [Flags]
        public enum BlizzardTargetMask
        {
            Ally = 1 << 0,
            Enemy = 1 << 1,
            InAnimate = 1 << 2,
        }

        //Inspector
        [SerializeField] BhaveInt blizzardVar;
        [SerializeField] int maxBlizzardValue = 5;

        [Header("Damage")]
        [SerializeField, EnumFlags] BlizzardTargetMask targetMask;
        [SerializeField, Range(1, 10)] int damage = 1;
        [SerializeField] UnitRegistry unitRegistry;

        [Space]
        [Header("Events")]
        [SerializeField] UnityEvent OnExecute;
        [SerializeField] UnityEvent OnReset;


    #region Core
        void Awake()
        {
            //Find unit registry if nothing passed in
            if (!unitRegistry) unitRegistry = FindObjectOfType<UnitRegistry>();
            Debug.Assert(unitRegistry, "No unit registry found!");
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

            //ALLIES
            if ((targetMask & BlizzardTargetMask.Ally) == BlizzardTargetMask.Ally)
            {
                //Damage all ally units
                var allyUnits = unitRegistry.GetUnits<AllyUnit>();
                DealDamageToUnits(allyUnits);
            }

            //ENEMIES
            if ((targetMask & BlizzardTargetMask.Enemy) == BlizzardTargetMask.Enemy)
            {
                //Deal damage to all enemies
                var enemyUnits = unitRegistry.GetUnits<EnemyUnit>();
                DealDamageToUnits(enemyUnits);
            }

            //OTHER (ie. Spirit crystals etc)
            if ((targetMask & BlizzardTargetMask.InAnimate) == BlizzardTargetMask.InAnimate)
            {
                //Deal blizzard damage to all inanimate enemies
                Debug.LogError("Blizzard affect on inanimate units not implemented!");
            }

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