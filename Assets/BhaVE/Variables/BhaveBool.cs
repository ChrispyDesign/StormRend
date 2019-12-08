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
	[CreateAssetMenu(menuName = "BhaVE/Variable/Bool", fileName = "BhaveBool")]
	public sealed class BhaveBool : BhaveVar<bool>
	{
		public static implicit operator BhaveBool(bool rhs)
		{
			return new BhaveBool{ value = rhs };
		}
		public static implicit operator bool(BhaveBool self)
		{
			return self.value;
		}
	}
}
