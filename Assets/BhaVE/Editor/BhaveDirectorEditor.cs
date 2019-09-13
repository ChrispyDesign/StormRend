using UnityEditor;
using UnityEngine;
using BhaVE.Core;

namespace BhaVE.Editor
{
	[CustomEditor(typeof(BhaveDirector))]
	public class BhaveDirectorEditor : UnityEditor.Editor
	{
		BhaveDirector t;

		void OnEnable()
		{
			t = target as BhaveDirector;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
				//Draw a debug manual tick button
				if (GUILayout.Button("Tick"))
				{
					t.Tick();
				}
				GUILayout.Space(5);
				if (GUILayout.RepeatButton("Turbo"))
				{
					t.Tick();
				}
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
		}
	}
}