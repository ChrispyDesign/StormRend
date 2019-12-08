/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using System;
using StormRend.Abilities;
using StormRend.MapSystems.Tiles;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine.Events;

namespace StormRend.Utility.Events
{
	[Serializable] public class StateEvent : UnityEvent<State> {}
	[Serializable] public class UnitEvent : UnityEvent<Unit> {}
	[Serializable] public class AbilityEvent : UnityEvent<Ability> {}
	[Serializable] public class TileEvent : UnityEvent<Tile> {}
	[Serializable] public class EffectEvent : UnityEvent<Effect> {}
	[Serializable] public class HealthEvent : UnityEvent<HealthData> {}
}