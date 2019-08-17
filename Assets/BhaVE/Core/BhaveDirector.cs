using UnityEngine;
using BhaVE.Patterns;
using BhaVE.Core.Enums;

namespace BhaVE.Core
{
	[CreateAssetMenu(menuName = "StormRend/AI Director", fileName = "AI Director")]
	public class BhaveDirector : ScriptableSingleton<BhaveDirector>
	{
		#region System settings
		[Tooltip("System tick interval method")]
		[SerializeField] TickMode _tickMode = TickMode.Fixed;
		public TickMode tickMode { 
			get => _tickMode; 
			set => _tickMode = value; }

		[Tooltip("System tick interval in seconds")]
		public float tickRate = 0.2f;
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

		/// <summary>  Tick the entire system  </summary>
		public void Tick()
		{
			foreach (var a in agentsRuntime)
				a.FullTick();
		}

		/// <summary> Tick a specific agent  </summary>
		/// <param name="agent">Agent to tick</param>
		public void Tick(BhaveAgent agent)  //TODO this is pointless because the agent's API lets you tick it
		{
			agent.FullTick();
		}

		/// <summary> Execute a paused tick for all agents </summary>
		/// <param name="paused">Pause status</param>
		public void PausedTick(bool paused)
		{
			foreach (var a in agentsRuntime)
				a.PauseTick(paused);
		}

		/// <summary> Execute a paused tick for a specific agent </summary>
		/// <param name="paused">Pause status</param>
		/// <param name="agent">Agent to tick</param>
		public void PausedTick(bool paused, BhaveAgent agent)
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
