/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine;

namespace StormRend.Assists.Designer
{
	public class TriggerDisplay : MonoBehaviour
	{
		//Set public variables to change the colour of the trigger display box
		public Color lineColor = Color.red;
		public Color fillColor = Color.red;

		//draw a box that is only visible in Editor view, good for debugging
		void OnDrawGizmos()
		{
			GetComponent<BoxCollider>().isTrigger = true;
			Vector3 drawBoxVector = new Vector3(
				this.transform.lossyScale.x * this.GetComponent<BoxCollider>().size.x,
				this.transform.lossyScale.y * this.GetComponent<BoxCollider>().size.y,
				this.transform.lossyScale.z * this.GetComponent<BoxCollider>().size.z
				);

			Vector3 drawBoxposition = this.transform.position + this.GetComponent<BoxCollider>().center;

			Gizmos.matrix = Matrix4x4.TRS(drawBoxposition, this.transform.rotation, drawBoxVector);
			Gizmos.color = fillColor;
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
			Gizmos.color = lineColor;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}
	}
}
