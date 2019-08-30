using UnityEngine;

namespace StormRend.Abilities.Effects
{
    public class TauntEffect : Effect
    {
        [SerializeField] int m_durationInTurns = 1;
        [SerializeField] int m_damageOnHit = 1;
    }
}