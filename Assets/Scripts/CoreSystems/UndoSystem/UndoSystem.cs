using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Defunct
{
	public class UndoSystem : MonoBehaviour
	{
		//Stop calling everything managers!
		//Maybe UndoSystem or just Undo
		//Static
		//Doesn't need to be a monobehaviour, maybe a scriptable object

		public List<ICommand> commands = new List<ICommand>();

		public void ClearCommands()
		{
			commands.Clear();
		}
	}
}
