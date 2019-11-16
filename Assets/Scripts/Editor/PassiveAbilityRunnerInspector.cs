using StormRend.Abilities.Utilities;
using UnityEditor;

namespace StormRend.Editors
{
	[CustomEditor(typeof(PassiveAbilityRunnerOnUnitTrade))]
    public class PassiveAbilityRunnerInspector : SmartEditor
    {
		string help = "Caches all passive abilities and runs them when unit is created or killed\n"; 
		public override string[] propertiesToExclude => new[] { "m_Script" };
        public override void OnPreInspector()
		{
			EditorGUILayout.HelpBox(help, MessageType.Info);
		}
    }
}