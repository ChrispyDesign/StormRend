using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMode
{
    IDLE,
    ATTACK,
    MOVE,

    COUNT
}

public class PlayerController : MonoBehaviour
{
    private PlayerUnit m_currentPlayer;
    private PlayerMode m_curMode;
    private bool m_isAbilityLocked;

    #region GettersAndSetters
    public PlayerUnit GetCurrentPlayer() { return m_currentPlayer; }
    public PlayerMode GetCurrentMode() { return m_curMode; }
    public bool GetIsAbilityLocked() { return m_isAbilityLocked; }

    public void SetCurrentPlayer(PlayerUnit _currentPlayer) { m_currentPlayer = _currentPlayer; }
    public void SetCurrentMode(PlayerMode _curMode) { m_curMode = _curMode; }
    public void SetIsAbilityLocked(bool _isLocked) { m_isAbilityLocked = _isLocked; }
    #endregion

    private void Start()
    {
        m_curMode = PlayerMode.IDLE;
    }
    private void Update()
    {
        Debug.Log(m_curMode.ToString());
    }
}
