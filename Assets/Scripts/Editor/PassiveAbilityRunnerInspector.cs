using StormRend.Abilities.Utilities;
using UnityEditor;

namespace StormRend.Editors
{
    [CustomEditor(typeof(PassiveAbilityRunner))]
    public class PassiveAbilityRunnerInspector : SmartEditor
    {
        string help = "Caches all passive abilities and runs them where appropriate\n" +
                    	"ie. on unit creation, unit killed or unit moved";
        public override string[] propertiesToExclude => new[] { "m_Script" };
        public override void OnPreInspector()
        {
            EditorGUILayout.HelpBox(help, MessageType.Info);
        }
    }
}