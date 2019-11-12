using System;
using StormRend.Abilities;
using StormRend.Systems.StateMachines;
using StormRend.Units;
using UnityEngine.Events;

namespace StormRend.Utility.Events
{
	[Serializable] public class StateEvent : UnityEvent<State> {}
	[Serializable] public class UnitEvent : UnityEvent<Unit> {}
	[Serializable] public class AbilityEvent : UnityEvent<Ability> {}
	[Serializable] public class EffectEvent : UnityEvent<Effect> {}
	[Serializable] public class HealthEvent : UnityEvent<HealthData> {}
}