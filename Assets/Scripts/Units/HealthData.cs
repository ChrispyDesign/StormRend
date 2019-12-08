/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System;

namespace StormRend.Units
{
	[Serializable]
	public struct HealthData
	{
		public Unit vendor;
		public int amount;

		public HealthData(Unit vendor, int amount)
		{
			this.vendor = vendor;
			this.amount = amount;
		}
	}
}
