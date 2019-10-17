using System.Collections.Generic;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Systems
{
    public class UndoSystem : MonoBehaviour
	{
		[ReadOnlyField]
		public List<ICommand> commands = new List<ICommand>();

		public void ClearCommands()
		{
			commands.Clear();
		}
	}
}