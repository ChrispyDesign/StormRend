using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;

public enum PlayerMode
{
    IDLE,
    ATTACK,
    MOVE,

    COUNT
}

public class UserInputHandler : MonoBehaviour
{
	//Rename:
	// - UserInputHandler
	// - GameplayInteractionHandler

    PlayerUnit currentPlayer;
    PlayerMode currentMode;
    bool isAbilityLocked;

    #region GettersAndSetters
    public PlayerUnit GetCurrentPlayer() { return currentPlayer; }
    public PlayerMode GetCurrentMode() { return currentMode; }
    public bool GetIsAbilityLocked() { return isAbilityLocked; }

    public void SetCurrentPlayer(PlayerUnit _currentPlayer) { currentPlayer = _currentPlayer; }
    public void SetCurrentMode(PlayerMode _curMode) { currentMode = _curMode; }
    public void SetIsAbilityLocked(bool _isLocked) { isAbilityLocked = _isLocked; }
    #endregion

    void Start()
    {
        currentMode = PlayerMode.IDLE;
    }

    string oldMode;
    void Update()
    {
        var newMode = currentMode.ToString();
        if (newMode != oldMode)
        {
            // Debug.Log(m_curMode.ToString());
            oldMode = newMode;
        }
    }
}
