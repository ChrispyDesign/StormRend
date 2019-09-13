using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BhaVE.Nodes;
using BhaVE.Core.Enums;
using BhaVE.Variables;
using UnityEngine.Events;

namespace BhaVE.Core
{
	public class BhaveAgent : MonoBehaviour
	{
		//Internal inbuilt tree of this agent.
		//Protected internal so that the BhaVEditor can access + subclasses
		[SerializeField] protected internal BhaveTree _internalTree = null;
		public BhaveTree internalTree => _internalTree;
		private BhaveTree backupTree = null;    //Backup tree so that module does not override any internal trees

		//Reference to external behaviour tree module.
		public NodeState? agentStatus { get; private set; }  //Needs to be set after each evaluation of the tree

		#region Inspector
		[SerializeField] string agentName = "BhaveAgent";
		[SerializeField] [TextArea] string description;

		[Header("Settings")]    //----------------------------------------------------------------------
		[SerializeField] bool _active = false;      //playOnAwake will set this to true automatically
		public bool isActive => _active;
		public void SetActive(bool active)
		{
			//Set and invoke appropriate events
			if (_active == false && active == true)          //Change from inactive to active
				OnActivate?.Invoke();
			else if (_active == true && active == false)     //Change from active to inactive
				OnDeactivate?.Invoke();

			_oldActive = _active;
			_active = active;
		}
		private bool _oldActive;

		[SerializeField] bool _paused = false;
		public bool isPaused => _paused;
		public void SetPaused(bool paused)
		{
			//Set and invoke appropriate events
			if (_paused == false && paused == true)      //Change from unpaused to paused
				OnPaused?.Invoke();
			else if (_paused == true && paused == false)     //Change from paused to unpaused
				OnUnpaused?.Invoke();

			_oldPaused = _paused;
			_paused = paused;    //Pause or unpause this agent's behaviour
		}
		private bool _oldPaused;

		[Tooltip("If true, the agent will start running once activated")]
		[SerializeField] bool playOnStart = true;

		[Tooltip("The agent will restart its behaviour when it has completed execution. Otherwise the agent will deactivate")]
		[SerializeField] bool restartWhenDone = true;

		[Tooltip("Agent will pause behaviour if agent deactivated. Otherwise will end behaviour")]
		[SerializeField] bool pauseIfDeactivated = false;


		[Header("Variables")]   //--------------------------------------------------------------------------
		public List<BhaveVarSeed> variables = new List<BhaveVarSeed>();     //TODO need it's own editor script


		[Header("Tree")]    //-------------------------------------------------------------------------------
		[SerializeField] BhaveTree bhaveTreeModule = null;  //THIS MUST NOT BE OVERRIDDEN!!
		private BhaveTree _READONLY_bhaveTreeModule => bhaveTreeModule; //Use this instead

		[Tooltip("Make a copy of the tree module instead of using it directly")]
		[SerializeField] bool useCopy = true;


		[Header("Events")]  //-------------------------------------------------------------------------------
		public Action OnActivate, OnDeactivate;
		public Action OnPaused, OnUnpaused;
		#endregion

		#region Inits
		void Awake()
		{
			//Start running agent if flagged
			_active = playOnStart;

			SelectTreeToUse();

			//Initiate and pass in ref to this agent (regardless of active status)
			_internalTree?.RunAwaken(this);
		}
		void OnEnable()
		{
			//Setup old values
			_oldActive = _active;
			_oldPaused = _paused;

			//Register events
			OnActivate += (CallbackActivate);
			OnDeactivate += (CallbackDeactivate);
			OnPaused += (CallbackPaused);
			OnUnpaused += (CallbackUnpaused);
		}
		void Start()
		{
			_internalTree?.RunInitiate(this);

			//Only start ticking once all the init methods have been executed
			StartCoroutine(FixedTickRoutine());
		}

		/// <summary>
		/// Select between either internal tree or external tree module
		/// </summary>
		void SelectTreeToUse()
		{
			try
			{
				//If there is an external tree
				if (_READONLY_bhaveTreeModule)
				{
					//Backup existing internal tree
					backupTree = _internalTree;

					//Make a copy of the external tree if set to copy
					if (useCopy)
						//TODO This does not do a deep copy!!! ie. Leaf nodes still reference the original tree's delegate objects
						//TEMP WORKAROUND Agents that need to have the same BhaveTree will need to each reference a full duplicate copy of the original BhaveTree
						_internalTree = Instantiate(_READONLY_bhaveTreeModule);

					//Otherwise set to use the external tree directly
					else
						_internalTree = _READONLY_bhaveTreeModule;
				}
				//Otherwise try the internal tree
				else if (_internalTree == null)
				{
					throw new NullReferenceException("No BhaVETree found! Shutting down agent...");
				}

				//Finally, internal tree is available for use
				//No more null checks required for internal tree
			}
			catch (Exception e)
			{
				Debug.LogWarning(e.Message);
				_active = false;
			}
		}
		#endregion

		#region Core
		// --------------- TickMode.Fixed --------------
		/// <summary>
		/// This coroutine is always started at enabled and runs constantly in the background during runtime
		/// </summary>
		protected virtual IEnumerator FixedTickRoutine()
		{
			while (true)
			{
				//Must be in fixed tick mode and agent active
				if (BhaveDirector.singleton.tickMode == TickMode.Fixed && _active)
				{
					// Debug.Log("Coroutine Tick: " + this.name);
					CircumstantialTick();

					//Deactivate agent if set to not restart when evaluated
					if (!restartWhenDone)
						SetActive(false);	//_agent = false; stacks and overlaps coroutines

					yield return new WaitForSeconds(BhaveDirector.singleton.tickRate);
				}
				else
				{
					//Not fixed mode
					yield return null;
				}
			}
		}
		//----------------- TickMode.EveryFrame -------------
		/// <summary>
		/// If TickMode is EveryFrame this agent will run it's Bhaviour every frame at full speed
		/// </summary>
		protected virtual void Update()
		{
			//Agent must be active and in EveryFrame tickmode
			if (BhaveDirector.singleton.tickMode != TickMode.EveryFrame || !_active)
				return;

			// Debug.Log("Update Tick: " + this.name);
			CircumstantialTick();

			//Stop agent if restart flag false
			if (!restartWhenDone)
				SetActive(false);
		}
		//----------------- TickMode.Manual ----------------
		//User can access this method through BhaveManager.Tick() (Cleaner API)

		/// <summary>
		/// Runs the correct tick based on whether the agent is paused or not
		/// </summary>
		internal void CircumstantialTick()
		{
			if (_paused)
				PauseTick(_paused);
			else
				FullTick();
		}
		/// <summary>
		/// Runs core BhaveTree execution stack (Begin, Execute, End)
		/// </summary>
		internal void FullTick()
		{
			// Debug.Log("Full Tick: " + this.name);
			_internalTree?.RunBegin();
			agentStatus = _internalTree?.RunExecute(this);
			_internalTree?.RunEnd();

			HandleSystemNodeStates();
		}
		/// <summary>
		/// Runs core BhaveTree pause execution
		/// </summary>
		internal void PauseTick(bool paused)	//TODO Does the paused bool need to be passed in?
		{
			// Debug.Log("Pause Tick: " + this.name);
			_internalTree?.RunPause(paused);

			HandleSystemNodeStates();
		}
		/// <summary>
		/// Handle agent deactivate and pause on encounter with abort and suspend states
		/// </summary>
		void HandleSystemNodeStates()
		{
			switch (agentStatus)
			{
				case NodeState.None:    //Failsafe: Shut everything down anyways. Should already be out of play mode
				case NodeState.Aborted:
					SetActive(false);   //Shut down immediately
					break;
				case NodeState.Suspended:
					SetPaused(true);    //Pause immediately
					break;
			}
		}
		#endregion

		#region Shutdown
		void OnDisable()
		{
			//Disable in reverse order
			StopCoroutine(FixedTickRoutine());

			//Unregister events
			OnUnpaused -= CallbackUnpaused;
			OnPaused -= CallbackPaused;
			OnDeactivate -= CallbackDeactivate;
			OnActivate -= CallbackActivate;

			//Restore tree (should be ok to overwrite as bhaveTreeModule is separate)
			_internalTree = backupTree;
		}
		void OnDestroy()
		{
			_internalTree?.RunShutdown();
		}
		#endregion

		#region Callbacks
		/// <summary>
		/// Handles inspector changes only in editor
		/// </summary>
		void OnValidate()
		{
			//Active > Inactive
			if (_oldActive == true && _active == false)
			{
				// SetActive(true);
				OnDeactivate?.Invoke();
			}
			//Inactive > Active
			else if (_oldActive == false && _active == true)
			{
				// SetActive(false);
				OnActivate?.Invoke();
			}
			//Unpaused > Paused
			else if (_oldPaused == false && _paused == true)
			{
				OnPaused?.Invoke();
			}
			//Paused > Unpaused
			else if (_oldPaused == true && _paused == false)
			{
				OnUnpaused?.Invoke();
			}
			//Record old values
			_oldActive = _active;
			_oldPaused = _paused;
		}
		protected void CallbackActivate()
		{
			// Debug.Log("Activated: " + this.name);
			// Debug.Log("Start Coroutine");
			StartCoroutine(FixedTickRoutine());
		}
		protected void CallbackDeactivate()
		{
			// Debug.Log("Deactivated: " + this.name);
			// Debug.Log("Stop Coroutine");
			StopAllCoroutines();

			if (pauseIfDeactivated)
				_paused = true;
		}
		protected void CallbackPaused()
		{
			// Debug.Log("Paused: " + this.name);
		}
		protected void CallbackUnpaused()
		{
			// Debug.Log("UnPaused: " + this.name);
		}
		#endregion
	}
}

//Q. Who should be able to access this?
//A. (public) External classes? [No], The user doesn't need to have direct access
//A. (internal) Other classes in the assembly? [Yes] so that the editor can access it
//	UPDATE: BHEditor should be in an Editor folder in it's own separate assembly. Therefore appropriate classes/methods/fields need to be public and assessible by the editor
//A. (protected) Sub classes? [YES] Sure why not. This class may be inherited in the future for some reason

//Brainstorm
//What is this class? What does it do?
// - Selecting either internal or external tree to used on startup
// - Can enabled/disabled it's bhave tree
// - Is able to pause its bhave tree's execution
// - Ticking and running it's internal behaviour tree
// - Holds an internal BhaveTree and can take in an external BhaveTree module
