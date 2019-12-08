/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.Systems;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Assists
{
	[RequireComponent(typeof(Unit))]
	public class UnitSelector : MonoBehaviour
	{
		[SerializeField] KeyCode selectKey = KeyCode.Alpha0;
		AnimateUnit au = null;
		UserInputHandler uih = null;

		void Awake()
		{
			au = GetComponent<AnimateUnit>();
			uih = UserInputHandler.current;
		}

		void Update()
		{
			if (Input.GetKeyDown(selectKey))
				if (!au.isDead)
					uih.SelectUnit(au, true);
		}
   	}
}