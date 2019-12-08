/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;

namespace StormRend.Sequencing
{
	/// <summary>
	/// Attach to objects that will be used to set off sequence trigger zones
	/// </summary>
	[RequireComponent(typeof(Collider))]
	public class TriggerObject : MonoBehaviour
	{
		void Awake()
		{
			var col = GetComponent<Collider>();
			Debug.Assert(col, "No collider attached to trigger object!");
		}
	}
}