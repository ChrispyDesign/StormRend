/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

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