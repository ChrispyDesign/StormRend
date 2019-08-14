using BhaVE;
using BhaVE.Editor.Settings;
using UnityEditor;
using UnityEngine;
namespace BhaVE.Editor
{
	[CustomEditor(typeof(BHESettings))]
	public class BHESettingsEditor : UnityEditor.Editor
	{
		SerializedProperty nodeSizeProp;
		SerializedProperty graphBGColorProp, rootColorProp, selColorProp, seqColorProp, decColorProp, condColorProp, actColorProp;
		SerializedProperty conStyleProp, lineThickProp, lineColorProp, bezTangProp;

		void OnEnable()
		{
			nodeSizeProp = serializedObject.FindProperty("nodeSize");

			graphBGColorProp = serializedObject.FindProperty("graphBGColor");
			rootColorProp = serializedObject.FindProperty("rootColor");
			selColorProp = serializedObject.FindProperty("selectorColor");
			seqColorProp = serializedObject.FindProperty("sequenceColor");
			decColorProp = serializedObject.FindProperty("decoratorColor");
			condColorProp = serializedObject.FindProperty("conditionColor");
			actColorProp = serializedObject.FindProperty("actionColor");

			conStyleProp = serializedObject.FindProperty("connectionStyle");
			lineThickProp = serializedObject.FindProperty("lineThickness");
			lineColorProp = serializedObject.FindProperty("lineColor");
			bezTangProp = serializedObject.FindProperty("bezierTangent");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			// serializedObject.Update();

			// EditorGUILayout.LabelField("Core", BhaVEditor.skin.textArea);
			// EditorGUILayout.PropertyField(nodeSizeProp);

			// EditorGUILayout.LabelField("Colors", BhaVEditor.skin.textArea);
			// EditorGUILayout.PropertyField(graphBGColorProp);
			// GUILayout.Label("Nodes", EditorStyles.boldLabel);
			// EditorGUILayout.PropertyField(rootColorProp);
			// EditorGUILayout.PropertyField(selColorProp);
			// EditorGUILayout.PropertyField(seqColorProp);
			// EditorGUILayout.PropertyField(decColorProp);
			// EditorGUILayout.PropertyField(condColorProp);
			// EditorGUILayout.PropertyField(actColorProp);

			// EditorGUILayout.LabelField("Connection Settings", BhaVEditor.skin.textArea);
			// EditorGUILayout.PropertyField(conStyleProp);
			// lineThickProp.floatValue = EditorGUILayout.Slider("Line Thickness", lineThickProp.floatValue, 1, 10);
			// EditorGUILayout.PropertyField(lineColorProp);
			// bezTangProp.floatValue = EditorGUILayout.Slider("Bezier Tangent", bezTangProp.floatValue, 0f, 100f);

			// serializedObject.ApplyModifiedProperties();
		}
	}
}