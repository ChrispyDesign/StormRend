using UnityEngine;
using UnityEngine.Events;
using StormRend.Utility.Attributes;
using StormRend.Units;
using pokoro.BhaVE.Core.Variables;
using StormRend.Enums;

namespace StormRend.Systems
{
	/// <summary>
	/// Applies blizzard to selected unit types
	/// </summary>
	public class BlizzardController : MonoBehaviour
    {
        //Inspector
        [SerializeField] BhaveInt blizzardVar;
        [SerializeField] int maxBlizzardValue = 5;

        [Header("Damage")]
        [SerializeField, EnumFlags] TargetUnitMask targetUnits;
        [SerializeField, Range(1, 10)] int damage = 1;

        [Space]
        [Header("Events")]
        [SerializeField] UnityEvent OnExecute;
        [SerializeField] UnityEvent OnReset;

        UnitRegistry ur;

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

            //ALLIES
            if ((targetUnits & TargetUnitMask.Allies) == TargetUnitMask.Allies)
            {
                //Damage all ally units
                var allyUnits = ur.GetUnits<AllyUnit>();
                DealDamageToUnits(allyUnits);
            }

            //ENEMIES
            if ((targetUnits & TargetUnitMask.Enemies) == TargetUnitMask.Enemies)
            {
                //Deal damage to all enemies
                var enemyUnits = ur.GetUnits<EnemyUnit>();
                DealDamageToUnits(enemyUnits);
            }

            //CRYSTALS
            if ((targetUnits & TargetUnitMask.Crystals) == TargetUnitMask.Crystals)
            {
                //Deal damage to all enemies
                var crystalUnits = ur.GetUnits<CrystalUnit>();
                DealDamageToUnits(crystalUnits);
            }

            //OTHER (ie. Spirit crystals etc)
            if ((targetUnits & TargetUnitMask.InAnimates) == TargetUnitMask.InAnimates)
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