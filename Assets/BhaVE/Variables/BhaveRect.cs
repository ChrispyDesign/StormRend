/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;
namespace pokoro.BhaVE.Core.Variables
{
	[CreateAssetMenu(menuName = "BhaVE/Variable/Rect", fileName = "BhaveRect")]
	public sealed class BhaveRect : BhaveVar<Rect>
	{
		public static implicit operator BhaveRect(Rect rhs)
		{
			return new BhaveRect{ value = rhs };
		}
		public static implicit operator Rect(BhaveRect self)
		{
			return self.value;
		}
	}
}
