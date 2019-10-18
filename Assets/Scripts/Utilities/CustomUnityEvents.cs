using System;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine.Events;

namespace StormRend.Utility.Events
{
	[Serializable] public class StateEvent : UnityEvent<State> {}
	[Serializable] public class UnitEvent : UnityEvent<Unit> {}
}