using System;
using UnityEngine;
using BhaVE.Core;
#if UNITY_EDITOR
using BhaVE.Editor;
#endif

namespace BhaVE.Nodes.Decorators
{
	public sealed class Repeater : Decorator
	{
		[SerializeField] uint repeats = 100;     //Minimum of 1 otherwise what's the point?
		int repsDone = 0;

		/// <summary> Repeat indefinitely unless end on failure set to true </summary>
		public bool loopForever = false;

		/// <summary> Will end immediately if failure encountered </summary>
		public bool endOnFailure = false;
		bool active = true;

		#region Core
		//Resets
		void OnEnable()
		{
			repsDone = 0;
			active = true;
		}

		protected internal override NodeState OnExecute(BhaveAgent agent)
		{
			try
			{
				if (!child) throw new NullReferenceException("Repeater has no child!");

				NodeState result = NodeState.Failure;

				//Will always return failure if end on failure is true
				//and it had previous encountered a failure
				if (active)
				{
					if (repsDone < repeats || loopForever)
					{
						result = child.OnExecute(agent);
						repsDone++;

					}

					//If endOnFailure set then this node will always return failure afterwards
					if (endOnFailure && result == NodeState.Failure)
					{
						active = false;
					}
				}

				state = result;
				return state;
			}
			catch (Exception e)
			{
				Debug.LogWarning(e.Message);
				state = NodeState.Failure;
				return state;
			}
		}
		#endregion

#if UNITY_EDITOR
		public override void DrawContent()
		{
			GUILayout.Label("@", BhaVEditor.skin.label);

			if (state == NodeState.None)
				GUILayout.Label("Rpts: " + repeats.ToString());
			else
				GUILayout.Label("Reps: " + repsDone.ToString());
		}
#endif
	}
}