using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealType
{
    Health,
    Rune,
    Curse
}

[CreateAssetMenu(fileName = "Heal", menuName = "StormRend/Effects/Heal")]
public class HealEffect : Effect
{
    public HealType m_healType;
    public int m_healAmount;
}
