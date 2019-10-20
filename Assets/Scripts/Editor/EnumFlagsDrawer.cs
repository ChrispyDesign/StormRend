using UnityEditor;
using UnityEngine;
using StormRend.Utility.Attributes;

namespace StormRend.Editors.PropDrawers
{
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
        }
    }
}
