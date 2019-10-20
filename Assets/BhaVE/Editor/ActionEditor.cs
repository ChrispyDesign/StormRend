using UnityEditor;
using BhaVE.Nodes.Leafs;
using BhaVE.Delegates;

namespace BhaVE.Editor
{
	[CustomEditor(typeof(Action))]
	public class ActionEditor : UnityEditor.Editor
	{
		Action t;
		void OnEnable()
		{
			t = target as Action;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			
			// t.SetDelegate(EditorGUILayout.ObjectField("Action Delegate", t.deleg, typeof(BhaveAction), false) as BhaveAction);
		}
	}
}
