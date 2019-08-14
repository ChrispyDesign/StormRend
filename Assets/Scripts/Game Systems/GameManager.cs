using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//Scriptable object singleton?

    private  static GameManager m_instance;

    [Header("Managers")]
    [SerializeField] private TurnManager m_turnManager = null;
    [SerializeField] private PlayerController m_playerController = null;
    [SerializeField] private CommandManager m_commandManager = null;

    [SerializeField] private PlayerUnit[] m_players;
    [SerializeField] private EnemyUnit[] m_enemies;
    [SerializeField] private List<Crystal> m_crystal;
	public int m_playerCount;
	public int m_enemyCount;

    #region GettersAndSetters
	public void AddCrystal(Crystal _crystal) { m_crystal.Add(_crystal); }

    public TurnManager GetTurnManager() { return m_turnManager; }
    public PlayerController GetPlayerController() { return m_playerController; }
    public CommandManager GetCommandManager() { return m_commandManager; }
	public PlayerUnit[] GetPlayerUnits() { return m_players; }
	public EnemyUnit[] GetEnemyUnits() { return m_enemies; }
	public List<Crystal> GetCrystals() { return m_crystal; }

    #endregion


    void Start()
    {
        Debug.Assert(m_turnManager, "Turn Manager not assigned to GameManager!");
        Debug.Assert(m_playerController, "Player Controller not assigned to GameManager!");
        Debug.Assert(m_commandManager, "Command Manager not assigned to GameManager!");
		m_playerCount = m_players.Length;
		m_enemyCount = m_enemies.Length;
	}

    void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			foreach (PlayerUnit p in m_players)
				p.Die();
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			foreach (EnemyUnit e in m_enemies)
				e.Die();
		}
	}

	public void CheckEndCondition()
	{
		GameOver();
		GameWin();
	}

	public static GameManager GetInstance()
    {
        // if no instance is assigned...
        if (!m_instance)
            m_instance = FindObjectOfType<GameManager>(); // find the instance

		// error handling
        Debug.Assert(m_instance, "UI Manager not found!");

        // done
        return m_instance;
    }

	public void GameOver()
	{
		if (m_playerCount <= 0)
		{
			UIManager uiManager = UIManager.GetInstance();
			GameOver gameOver = uiManager.gameObject.GetComponentInParent<GameOver>();
			gameOver.ShowScreen();
		}
	}

	public void GameWin()
	{
		if (m_enemyCount <= 0)
		{
			UIManager uiManager = UIManager.GetInstance();
			GameWin gameWin = uiManager.gameObject.GetComponentInParent<GameWin>();
			gameWin.ShowScreen();
		}
	}
}
