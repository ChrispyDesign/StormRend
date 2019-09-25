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

public class PlayerController : MonoBehaviour
{
	//Rename:
	// - UserInputHandler
	// - GameplayInteractionHandler

	public PauseMenu m_pauseMenu;

    private PlayerUnit m_currentPlayer;
    private PlayerMode m_curMode;
    private PlayerMode m_prevMode;
    private bool m_isAbilityLocked;
    string oldMode;

    #region GettersAndSetters
    public PlayerUnit GetCurrentPlayer() { return m_currentPlayer; }
    public PlayerMode GetCurrentMode() { return m_curMode; }
    public PlayerMode GetPrevMode() { return m_prevMode; }
    public bool GetIsAbilityLocked() { return m_isAbilityLocked; }

    public void SetCurrentPlayer(PlayerUnit _currentPlayer) { m_currentPlayer = _currentPlayer; }
    public void SetCurrentMode(PlayerMode _curMode)
	{
		if (m_curMode == PlayerMode.IDLE)
			m_prevMode = PlayerMode.MOVE;
		else
			m_prevMode = m_curMode;

		m_curMode = _curMode;
	}
    public void SetIsAbilityLocked(bool _isLocked) { m_isAbilityLocked = _isLocked; }
    #endregion

    private void Start()
    {
        m_curMode = PlayerMode.IDLE;
    }

    private void Update()
    {
        var newMode = m_curMode.ToString();
        if (newMode != oldMode)
        {
            // Debug.Log(m_curMode.ToString());
            oldMode = newMode;
        }

		if (m_currentPlayer != null && Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(1))
		{
			m_currentPlayer.OnDeselect();
			m_currentPlayer = null;
		}
		else if (Input.GetKeyUp(KeyCode.Escape))
		{
			m_pauseMenu.GamePause();
		}
    }
}
