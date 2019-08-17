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

			GUILayout.Space(4);
			GUILayout.BeginHorizontal();
				//Draw a debug manual tick button
				if (GUILayout.Button(new GUIContent("Tick", "Tick all agents once")))
				{
					t.Tick();
				}
				GUILayout.Space(4);
				if (GUILayout.RepeatButton(new GUIContent("Turbo", "Hold to tick agents repeatedly")))
				{
					t.Tick();
				}
			GUILayout.EndHorizontal();
			GUILayout.Space(4);
		}
	}
}