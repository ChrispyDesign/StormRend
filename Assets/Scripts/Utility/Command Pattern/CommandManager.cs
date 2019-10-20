using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
	//Stop calling everything managers!
	//Maybe UndoSystem or just Undo
	//Static
	//Doesn't need to be a monobehaviour, maybe a scriptable object

	public List<ICommand> m_moves;

	private void Start()
	{
		m_moves = new List<ICommand>();
	}

	public static void RegisterMove(MoveCommand command)
	{

	}
}
