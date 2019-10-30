using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormRend.Defunct
{
	public interface xICommand
	{
		void Execute();
		void Undo();
	}
}