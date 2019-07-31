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
    private static PlayerUnit m_currentPlayer;
    private static PlayerMode m_curMode;

    #region GettersAndSetters
    public static PlayerUnit GetCurrentPlayer() { return m_currentPlayer; }
    public static PlayerMode GetCurrentMode() { return m_curMode; }

    public static void SetCurrentPlayer(PlayerUnit _currentPlayer) { m_currentPlayer = _currentPlayer; }
    public static void SetCurrentMode(PlayerMode _curMode) { m_curMode = _curMode; }
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
