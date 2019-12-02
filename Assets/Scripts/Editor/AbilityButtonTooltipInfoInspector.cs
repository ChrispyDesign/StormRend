using StormRend.UI;
using UnityEditor;

namespace StormRend.Editors
{
	[CustomEditor(typeof(AbilityButtonTooltipInfo))]
	public class AbilityButtonTooltipInfoInspector : SmartEditor
	{
		//Hide fields message and delay (Unsuccessful)
		public override string[] propertiesToExclude =>
			new[] { "message",  "delay" };
	}
}