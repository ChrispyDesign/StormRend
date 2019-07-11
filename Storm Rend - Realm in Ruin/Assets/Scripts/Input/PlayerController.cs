using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerUnit m_currentPlayer;
    
    public static PlayerUnit GetCurrentPlayer() { return m_currentPlayer; }
    public static void SetCurrentPlayer(PlayerUnit _currentPlayer) { m_currentPlayer = _currentPlayer; }
}
