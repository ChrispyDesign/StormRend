using UnityEditor;
using BhaVE.Nodes.Leafs;
using BhaVE.Delegates;

namespace BhaVE.Editor
{
	[CustomEditor(typeof(Condition))]
	public class ConditionEditor : UnityEditor.Editor
	{
		Condition t;	//Target
		void OnEnable()
		{
			t = target as Condition;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			
			// t.SetDelegate(EditorGUILayout.ObjectField("Condition Delegate", t.deleg, typeof(BhaveCondition), false) as BhaveCondition);
		}
	}
}
