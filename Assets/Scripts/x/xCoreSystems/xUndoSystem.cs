using System.Collections.Generic;
using StormRend.Utility.Attributes;
using UnityEngine;

namespace StormRend.Defunct
{
    public class xUndoSystem : MonoBehaviour
	{
		[ReadOnlyField]
		public List<xICommand> commands = new List<xICommand>();

		public void ClearCommands()
		{
			commands.Clear();
		}
	}
}