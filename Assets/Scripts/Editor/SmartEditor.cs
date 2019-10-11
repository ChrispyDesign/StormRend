using UnityEditor;

namespace StormRend.Editors
{
	public abstract class SmartEditor : Editor
	{
		public virtual void OnPreInspector() { }
		public virtual string[] propertiesToExclude => new string[0];
		public virtual void OnPostInspector() { }

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			OnPreInspector();
			DrawPropertiesExcluding(serializedObject, propertiesToExclude);
			OnPostInspector();

			serializedObject.ApplyModifiedProperties();
		}

	}
}