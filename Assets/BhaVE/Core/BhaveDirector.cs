using UnityEngine;
using BhaVE.Core.Enums;
using BhaVE.Patterns;

namespace BhaVE.Core
{
	public class BhaveDirector : Singleton<BhaveDirector>
	{
		#region System settings
		[Tooltip("System tick interval method")]
		[SerializeField] TickMode _tickMode = TickMode.Fixed;
		public TickMode tickMode {
			get => _tickMode;
			set => _tickMode = value; }

		[Tooltip("System tick interval in seconds")]
		public float tickRate = 0.15f;
		#endregion

		//Agents
		BhaveAgent[] agentsRuntime;
		public BhaveAgent[] GetAllAgents() => agentsRuntime;
		void FindAndCacheAllAgents()
		{
			agentsRuntime = FindObjectsOfType<BhaveAgent>();
		}

		#region Core
		void OnEnable()
		{
			FindAndCacheAllAgents();
		}

		/// <summary> Ticks the entire system. Respects each agent's pause status </summary>
		public void Tick()
		{
			foreach (var a in agentsRuntime)
			{
				a.CircumstantialTick();
			}
		}

		/// <summary> Tick a specific agent  </summary>
		/// <param name="agent">Agent to tick</param>
		public void Tick(BhaveAgent agent)
		{
			agent.CircumstantialTick();
		}

		/// <summary> Forced full tick for all agents ignoring their pause statuses </summary>
		public void ForcedTick()
		{
			foreach (var a in agentsRuntime)
				a.FullTick();
		}

		/// <summary> Forced full tick for a specific agent ignoring it's pause status </summary>
		/// <param name="agent">Agent to tick</param>
		public void ForceTick(BhaveAgent agent)
		{
			agent.FullTick();
		}

		/// <summary> Forced pause tick for all agents </summary>
		/// <param name="paused">Pause state to send through</param>
		public void ForcedPauseTick(bool paused = true)
		{
			foreach (var a in agentsRuntime)
				a.PauseTick(paused);
		}

		/// <summary> Forced pause tick for a specific agent </summary>
		/// <param name="paused">Pause state to send through</param>
		/// <param name="agent">Agent to tick</param>
		public void ForcedPauseTick(BhaveAgent agent, bool paused = true)
		{
			agent.PauseTick(paused);
		}
		#endregion
	}
}

//Brainstorm
//What is this class? What does it do?
// - Singleton MonoBehaviour accessible by all agents
// - Gets a reference to all agents
// - Stores system tick interval (seconds)
// - Doesn't need to be deactivated or active
// - Bhave system suspend flag
// API
// V Can tick the entire system
// V Can tick a specific agent Q. What's the point? Unless BhaveAgent.Tick() is internal. A: Better API ie. BhaveDirector.Tick(agent) vs. agent.FullTick(). Pause is also handled
// X Can return a specific agent: No point
// V Can return all agents
