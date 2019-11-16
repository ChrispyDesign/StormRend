using UnityEditor;

namespace StormRend.Editors
{
	public abstract class SmartEditor : Editor
	{
		public virtual string[] propertiesToExclude => new string[0];
		public virtual void OnPreInspector() { }
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			OnPreInspector();
			DrawPropertiesExcluding(serializedObject, propertiesToExclude);
			OnPostInspector();
			serializedObject.ApplyModifiedProperties();
		}
		public virtual void OnPostInspector() { }
	}
}