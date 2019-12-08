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
	[CreateAssetMenu(menuName = "BhaVE/Variable/Transform", fileName = "BhaveTranform")]
	public sealed class BhaveTransform : BhaveVar<Transform>
	{
		//This doesn't seem to work properly. Safer to set .value directly
		// public static implicit operator BhaveTransform(Transform rhs)
		// {
		// 	var ret = CreateInstance<BhaveTransform>();
		// 	ret.value = rhs;
		// 	return ret;
		// }

		public static implicit operator Transform(BhaveTransform self)
		{
			return self.value;
		}
	}
}
