using UnityEngine;

namespace StormRend.UI
{
	[RequireComponent(typeof(AbilityButton))]
	public class AbilityButtonTooltipInfo : TooltipInfo
	{
		//Properties
		protected override string message
		{
			get
			{
				if (!ab.ability) return null;
				
				string messageResult = null;
				foreach (var txt in ab.ability.descriptions)
				{
					messageResult += (txt == "") ? "" : (txt + "\n");
				}
				return messageResult;
			}
		}

		//Members
		AbilityButton ab = null;

		protected override void Awake()
		{
			base.Awake();
			ab = GetComponent<AbilityButton>();
		}
	}
}