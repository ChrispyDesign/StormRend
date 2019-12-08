/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System;

namespace StormRend.Enums
{
	/// <summary>
	/// Target and Unit type enum. Can be used as a bitmask
	/// </summary>
	[Flags]
	public enum TargetType
	{
		Empty = 1 << 0,     //Empty is when no bits are selected!
		Self = 1 << 1,
		Allies = 1 << 2,
		Enemies = 1 << 3,
		Crystals = 1 << 4,
		InAnimates = 1 << 5,    //Such as crystals
		Animates = 1 << 6,
	}
}