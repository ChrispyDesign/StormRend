using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using pokoro.Patterns.Generic;
using StormRend.Systems;

namespace StormRend.Defunct
{
	public class xGameManager : Singleton<xGameManager>
	{
		#region Inspectors
		public UnityEvent OnGameLose, OnGameWin;
		#endregion

		[Header("Managers")]
		// [SerializeField] TurnManager m_turnManager = null;
		[SerializeField] xPlayerController m_playerController = null;
		[SerializeField] UndoSystem m_commandManager = null;
		[SerializeField] xPlayerUnit[] m_players;
		[SerializeField] xEnemyUnit[] m_enemies;
		[SerializeField] List<xCrystal> m_crystal;

		#region GettersAndSetters
		public void AddCrystal(xCrystal _crystal) { m_crystal.Add(_crystal); }

		// public TurnManager GetTurnManager() { return m_turnManager; }
		public xPlayerController GetPlayerController() { return m_playerController; }
		public UndoSystem GetCommandManager() { return m_commandManager; }

		public xPlayerUnit[] GetPlayerUnits() { return m_players; }
		public xEnemyUnit[] GetEnemyUnits() { return m_enemies; }
		public List<xCrystal> GetCrystals() { return m_crystal; }

		#endregion

		void Awake()
		{
			//Populate unit lists
			m_enemies = FindObjectsOfType<xEnemyUnit>();
			m_players = FindObjectsOfType<xPlayerUnit>();

			DoAsserts();
		}

		void DoAsserts()
		{
			// Debug.Assert(m_turnManager, "Turn Manager not assigned to GameManager!");
			Debug.Assert(m_playerController, "Player Controller not assigned to GameManager!");
			Debug.Assert(m_commandManager, "Command Manager not assigned to GameManager!");
		}

		void Update()
		{
			//Debug kill all players
			if (Input.GetKeyDown(KeyCode.P))
			{
				foreach (xPlayerUnit p in m_players)
					p.Die();
			}

			//Debug kill all enemies
			if (Input.GetKeyDown(KeyCode.E))
			{
				foreach (xEnemyUnit e in m_enemies)
					e.Die();
			}
		}

		public void RegisterUnitDeath(xUnit deadUnit)
		{
			//Filter out dead unit and reassign back to appropriate unit list
			if (deadUnit is xPlayerUnit)
			{
				m_players = m_players.Where(x => x != deadUnit).ToArray();
			}
			else if (deadUnit is xEnemyUnit)
			{
				m_enemies = m_enemies.Where(x => x != deadUnit).ToArray();
			}

			//Then check for end conditions
			CheckGameEnd();
		}

		void CheckGameEnd()
		{
			CheckGameLose();
			CheckGameWin();
		}

		public void CheckGameLose()
		{
			if (m_players.Length <= 0)
				OnGameLose.Invoke();
		}

		public void CheckGameWin()
		{
			if (m_enemies.Length <= 0)
				OnGameWin.Invoke();
		}
	}

}