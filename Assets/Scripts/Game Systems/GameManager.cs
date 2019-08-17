using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;
using BhaVE.Patterns;

public class GameManager : Singleton<GameManager>
{
	//Scriptable object singleton?

    static GameManager m_instance;

    [Header("Managers")]
    [SerializeField] TurnManager m_turnManager = null;
    [SerializeField] PlayerController m_playerController = null;
    [SerializeField] CommandManager m_commandManager = null;

    [SerializeField] PlayerUnit[] m_players;
    [SerializeField] EnemyUnit[] m_enemies;
    [SerializeField] List<Crystal> m_crystal;
	public int playerCount { get; set; }
	public int enemyCount { get; set; }

    #region GettersAndSetters
	public void AddCrystal(Crystal _crystal) { m_crystal.Add(_crystal); }

    public TurnManager GetTurnManager() { return m_turnManager; }
    public PlayerController GetPlayerController() { return m_playerController; }
    public CommandManager GetCommandManager() { return m_commandManager; }
	public PlayerUnit[] GetPlayerUnits() { return m_players; }
	public EnemyUnit[] GetEnemyUnits() { return m_enemies; }
	public List<Crystal> GetCrystals() { return m_crystal; }

    #endregion

	protected override void Awake()
	{
		base.Awake();
		//Populate unit lists
		m_enemies = FindObjectsOfType<EnemyUnit>();
		m_players = FindObjectsOfType<PlayerUnit>();
	}


    void Start()
    {
        Debug.Assert(m_turnManager, "Turn Manager not assigned to GameManager!");
        Debug.Assert(m_playerController, "Player Controller not assigned to GameManager!");
        Debug.Assert(m_commandManager, "Command Manager not assigned to GameManager!");
		playerCount = m_players.Length;
		enemyCount = m_enemies.Length;
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

	public void GameOver()
	{
		if (playerCount <= 0)
		{
			UIManager uiManager = UIManager.GetInstance();
			GameOver gameOver = uiManager.gameObject.GetComponentInParent<GameOver>();
			gameOver.ShowScreen();
		}
	}

	public void GameWin()
	{
		if (enemyCount <= 0)
		{
			UIManager uiManager = UIManager.GetInstance();
			GameWin gameWin = uiManager.gameObject.GetComponentInParent<GameWin>();
			gameWin.ShowScreen();
		}
	}
}
