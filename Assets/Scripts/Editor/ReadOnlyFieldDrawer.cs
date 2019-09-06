using UnityEditor;
using UnityEngine;
using StormRend.Utility.Attributes;

namespace StormRend.Editors.PropDrawers
{
    [CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
    public class ReadOnlyFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var fieldType = property.type;

            switch (fieldType)
            {
                case "int":
                    EditorGUI.LabelField(position, label.text, property.intValue.ToString());
                    break;
                case "float":
                    EditorGUI.LabelField(position, label.text, property.floatValue.ToString());
                    break;
                case "bool":
                    EditorGUI.LabelField(position, label.text, property.boolValue.ToString());
                    break;
                case "string":
                    EditorGUI.LabelField(position, label.text, property.stringValue);
                    break;
                default:
                    EditorGUI.LabelField(position, label.text, "Property drawer not implemented for this type!");
                    break;
            }
        }
    }
}