/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using StormRend.MapSystems.Tiles;
using StormRend.Units;
using UnityEngine;

namespace StormRend.Abilities.Utilities
{
	[RequireComponent(typeof(AnimateUnit))]
	public class OnMovedPassiveAbilityRunner : MonoBehaviour
	{
		AnimateUnit au = null;

		void Awake() => au = GetComponent<AnimateUnit>();

		void OnEnable() => au.onMoved.AddListener(OnMoved);
		void OnDisable() => au.onMoved.RemoveListener(OnMoved);

		void OnMoved(Tile tile)
		{
			
		}
	}
}