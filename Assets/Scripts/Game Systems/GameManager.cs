using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;
using BhaVE.Patterns;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
	[Header("Managers")]
	[SerializeField] TurnManager turnManager = null;
	[SerializeField] PlayerController playerController = null;
	[SerializeField] CommandManager commandManager = null;
	[SerializeField] PlayerUnit[] players;
	[SerializeField] EnemyUnit[] enemies;
	[SerializeField] List<Crystal> crystals;
	public PlayerUnit sage { get; set; }		//???

#region Getters And Setters
	public void AddCrystal(Crystal _crystal) { crystals.Add(_crystal); }


	public TurnManager GetTurnManager() { return turnManager; }
	public PlayerController GetPlayerController() { return playerController; }
	public CommandManager GetCommandManager() { return commandManager; }

	public PlayerUnit[] GetPlayerUnits() { return players; }
	public EnemyUnit[] GetEnemyUnits() { return enemies; }
	public List<Crystal> GetCrystals() { return crystals; }

	#endregion

	protected override void Awake()
	{
		base.Awake();   //Automatic singleton setup

		//Populate unit lists
		enemies = FindObjectsOfType<EnemyUnit>();
		players = FindObjectsOfType<PlayerUnit>();

		foreach(PlayerUnit player in players)
		{
			if (player.unitType == PlayerClass.SAGE)
				sage = player;
		}

		DoAsserts();
	}

	void DoAsserts()
	{
		Debug.Assert(turnManager, "Turn Manager not assigned to GameManager!");
		Debug.Assert(playerController, "Player Controller not assigned to GameManager!");
		Debug.Assert(commandManager, "Command Manager not assigned to GameManager!");
	}

	void Update()
	{
		//Debug kill all players
		if (Input.GetKeyDown(KeyCode.P))
		{
			foreach (PlayerUnit p in players)
				p.Die();
		}

		//Debug kill all enemies
		if (Input.GetKeyDown(KeyCode.E))
		{
			foreach (EnemyUnit e in enemies)
				e.Die();
		}
	}

	public void RegisterUnitDeath(Unit deadUnit)
	{
		//Filter out dead unit and reassign back to appropriate unit list
		if (deadUnit is PlayerUnit)
		{
			players = players.Where(x => x != deadUnit).ToArray();
		}
		else if (deadUnit is EnemyUnit)
		{
			enemies = enemies.Where(x => x != deadUnit).ToArray();
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
		if (players.Length <= 0)
		{
			UIManager uiManager = UIManager.GetInstance();
			GameOver gameOver = uiManager.gameObject.GetComponentInParent<GameOver>();
			gameOver.ShowScreen();
		}
	}

	public void GameWin()
	{
		if (enemies.Length <= 0)
		{
			UIManager uiManager = UIManager.GetInstance();
			GameWin gameWin = uiManager.gameObject.GetComponentInParent<GameWin>();
			gameWin.ShowScreen();
		}
	}

	public void DeselectAllUnits<T>() where T : Unit
	{
		if (typeof(T) is EnemyUnit)
		{
			foreach (var e in enemies)
			{
				e.OnDeselect();
			}
		}
		else if (typeof(T) is PlayerUnit)
		{
			foreach (var p in players)
			{
				p.OnDeselect();
			}
		}
	}
}
