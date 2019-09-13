using BhaVE.Editor.Settings;
using UnityEditor;
namespace BhaVE.Editor
{
	[CustomEditor(typeof(BHEConfig))]
	public class BHEConfigEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}
}