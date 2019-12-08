/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using pokoro.Patterns.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StormRend.UI
{
	public sealed class MasterCanvas : Singleton<MasterCanvas>
	{
		public GraphicRaycaster graphicRaycaster { get; private set; }
		public Canvas canvas { get; private set; }
		void Awake()
		{
			canvas = GetComponent<Canvas>();
			graphicRaycaster = GetComponent<GraphicRaycaster>();
		}
	}
}