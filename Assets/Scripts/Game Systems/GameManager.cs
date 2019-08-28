using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;
using BhaVE.Patterns;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    [Header("Managers")]
    [SerializeField] TurnManager m_turnManager = null;
    [SerializeField] PlayerController m_playerController = null;
    [SerializeField] CommandManager m_commandManager = null;
    [SerializeField] PlayerUnit[] m_players;
    [SerializeField] EnemyUnit[] m_enemies;
    [SerializeField] List<Crystal> m_crystal;

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
        base.Awake();	//Automatic singleton setup

        //Populate unit lists
        m_enemies = FindObjectsOfType<EnemyUnit>();
        m_players = FindObjectsOfType<PlayerUnit>();

		DoAsserts();
    }

    void DoAsserts()
    {
        Debug.Assert(m_turnManager, "Turn Manager not assigned to GameManager!");
        Debug.Assert(m_playerController, "Player Controller not assigned to GameManager!");
        Debug.Assert(m_commandManager, "Command Manager not assigned to GameManager!");
    }

    void Update()
    {
        //Debug kill all players
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (PlayerUnit p in m_players)
                p.Die();
        }

        //Debug kill all enemies
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (EnemyUnit e in m_enemies)
                e.Die();
        }
    }

	public void RegisterUnitDeath(Unit deadUnit)
    {
        //Filter out dead unit and reassign back to appropriate unit list
        if (deadUnit is PlayerUnit)
        {
            m_players = m_players.Where(x => x != deadUnit).ToArray();
        }
        else if (deadUnit is EnemyUnit)
        {
            m_enemies = m_enemies.Where(x => x != deadUnit).ToArray();
        }

		//Then check for end conditions
		CheckGameEnd();
    }

    void CheckGameEnd()
    {
        GameOver();
        GameWin();
    }

    public void GameOver()
    {
		if (m_players.Length <= 0)
        {
            UIManager uiManager = UIManager.GetInstance();
            GameOver gameOver = uiManager.gameObject.GetComponentInParent<GameOver>();
            gameOver.ShowScreen();
        }
    }

    public void GameWin()
    {
        if (m_enemies.Length <= 0)
        {
            UIManager uiManager = UIManager.GetInstance();
            GameWin gameWin = uiManager.gameObject.GetComponentInParent<GameWin>();
            gameWin.ShowScreen();
        }
    }
}
