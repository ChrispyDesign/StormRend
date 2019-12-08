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
	[CreateAssetMenu(menuName = "BhaVE/Variable/Vector2", fileName = "BhaveVector2")]
	public sealed class BhaveVector2 : BhaveVar<Vector2>
	{
		public static implicit operator BhaveVector2(Vector2 rhs)
		{
			return new BhaveVector2 { value = rhs };
		}
		public static implicit operator Vector2(BhaveVector2 self)
		{
			return self.value;
		}
	}
}
