using UnityEngine;

public enum RefreshType
{
    AttackAgain,
    MoveAgain
}

public class RefreshEffect : Effect
{
    [SerializeField] private RefreshType m_refreshType;
}