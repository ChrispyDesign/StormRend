using UnityEditor;
using UnityEngine;
using StormRend.Utility.Attributes;

namespace StormRend.Editors.PropDrawers
{
	[CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
	public class ReadOnlyFieldDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
			=> EditorGUI.GetPropertyHeight(property, label, true);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var fieldType = property.type;
			switch (fieldType)
			{
				case "bool":
					EditorGUI.LabelField(position, label.text, property.boolValue.ToString());
					break;
				case "int":
					EditorGUI.LabelField(position, label.text, property.intValue.ToString());
					break;
				case "float":
					EditorGUI.LabelField(position, label.text, property.floatValue.ToString());
					break;
				case "double":
					EditorGUI.LabelField(position, label.text, property.doubleValue.ToString());
					break;
				case "rect":
					EditorGUI.LabelField(position, label.text, property.rectValue.ToString());
					break;
				case "bounds":
					EditorGUI.LabelField(position, label.text, property.boundsValue.ToString());
					break;
				case "color":
					EditorGUI.LabelField(position, label.text, property.colorValue.ToString());
					break;
				case "quaternion":
					EditorGUI.LabelField(position, label.text, property.quaternionValue.ToString());
					break;
				case "vector2":
					EditorGUI.LabelField(position, label.text, property.vector2Value.ToString());
					break;
				case "vector3":
					EditorGUI.LabelField(position, label.text, property.vector3Value.ToString());
					break;
				case "vector4":
					EditorGUI.LabelField(position, label.text, property.vector4Value.ToString());
					break;
				case "string":
					EditorGUI.LabelField(position, label.text, property.stringValue);
					break;
				default:
					GUI.enabled = false;
					EditorGUI.PropertyField(position, property, label, true);
					GUI.enabled = true;
					break;
			}
		}
	}
}