using System.Collections;
using System.Collections.Generic;
using StormRend.Abilities;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private int m_currentAbilityLevel;
    [SerializeField] private List<Ability> m_abilities;

    #endregion

    #region GettersAndSetters

    public int GetCurrentLevel() { return m_currentAbilityLevel; }
    public List<Ability> GetAbilities() { return m_abilities; }

    public void SetCurrentLevel(int _curLevel) { m_currentAbilityLevel = _curLevel; }

    #endregion


}
