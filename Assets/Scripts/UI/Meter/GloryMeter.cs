/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEngine.EventSystems;

namespace StormRend.UI
{
	public class GloryMeter : Meter
	{
		// public void UpdatePanel()
		// {
			// //Increase
			// if (currentValue < glory.value)
			// 	increase = true;

			// //Decrease
			// else if (currentValue > glory.value)
			// 	decrease = true;

			// currentValue = glory.value;

			// if (increase || startCheck)
			// {
			// 	for (int i = 0; i < glory.value; i++)
			// 	{
			// 		Debug.Log("isActiveandenabled: " + isActiveAndEnabled);
			// 		StartCoroutine(IncreaseGlory(i));
			// 	}
			// }

			// if (decrease)
			// {
			// 	for (int i = gloryNodes.Length - 1; i >= glory.value; i--)
			// 		StartCoroutine(DecreaseGlory(i));
			// }
		// }

		// IEnumerator IncreaseGlory(int _index)
		// {
		// 	if (gloryNodes[_index].fillAmount == 1)     //Checking float by equality BAD. Didn't cast to 
		// 	{
		// 		startCheck = false;
		// 		increase = false;
		// 		yield return null;
		// 	}
		// 	for (float i = 0f; i <= 1; i += fillSpeed)
		// 	{
		// 		gloryNodes[_index].fillAmount += fillSpeed;
		// 		yield return new WaitForSeconds(fillSpeed);
		// 	}
		// }
		// IEnumerator DecreaseGlory(int _index)
		// {
		// 	if (gloryNodes[_index].fillAmount == 1)
		// 	{
		// 		startCheck = false;
		// 		decrease = false;
		// 		yield return null;
		// 	}
		// 	for (float i = 0f; i <= 1; i += fillSpeed)
		// 	{
		// 		gloryNodes[_index].fillAmount -= fillSpeed;
		// 		yield return new WaitForSeconds(fillSpeed);
		// 	}
		// }
	}
}