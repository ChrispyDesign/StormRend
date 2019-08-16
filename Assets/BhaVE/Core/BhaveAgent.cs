using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BhaVE.Nodes;
using BhaVE.Core.Enums;
using BhaVE.Variables;

namespace BhaVE.Core
{

	public class BhaveAgent : MonoBehaviour
	{
		//Internal inbuilt tree of this agent. 	
		//Protected internal so that the BhaVEditor can access + subclasses
		[HideInInspector] [SerializeField] protected internal BhaveTree _internalTree = null;
		public BhaveTree internalTree => _internalTree;
		private BhaveTree backupTree = null;    //Backup tree so that module does not override any internal trees

		//Reference to external behaviour tree module.
		public NodeState agentStatus { get; private set; }  //Needs to be set after each evaluation of the tree

		#region Inspector
		[SerializeField] string agentName = "BhaveAgent";
		[SerializeField] [TextArea] string description;

		[Header("Settings")]
		[SerializeField] bool _active = false;      //playOnAwake will set this to true automatically
		public bool isActive => _active;
		public void SetActive(bool active)
		{
			//Set and invoke appropriate events
			if (_active == false && active == true)          //Change from inactive to active
				OnActivate.Invoke();
			else if (_active == true && active == false)     //Change from active to inactive
				OnDeactivate.Invoke();

			_active = active;
		}

		[SerializeField] bool _paused = false;
		public bool isPaused => _paused;
		public void SetPaused(bool paused)
		{
			//Set and invoke appropriate events
			if (_paused == false && paused == true)      //Change from unpaused to paused
				OnPause.Invoke();
			else if (_paused == true && paused == false)     //Change from paused to unpaused
				OnUnpause.Invoke();

			_paused = paused;    //Pause or unpause this agent's behaviour
		}

		[Tooltip("If true, the agent will start running once activated")]
		[SerializeField] bool playOnStart = true;

		[Tooltip("The agent will restart its behaviour when it has completed execution. Otherwise the agent will deactivate")]
		[SerializeField] bool restartWhenDone = true;

		[Tooltip("Agent will pause behaviour if agent deactivated. Otherwise will end behaviour")]
		[SerializeField] bool pauseIfDeactivated = false;

		[Header("Tree")]
		[SerializeField] BhaveTree bhaveTreeModule = null;  //THIS MUST NOT BE OVERRIDDEN!!
		private BhaveTree _READONLY_bhaveTreeModule => bhaveTreeModule; //Use this instead

		[Tooltip("Make a copy of the tree module instead of using it directly")]
		[SerializeField] bool useCopy = true;
		bool internalTreeSet = false;

		// [Header("Variables")]
		// public List<BhaveVar<object>> variables = new List<BhaveVar<object>>();     //TODO need it's own editor script

		[Header("Events")]
		public Action OnActivate = delegate { };
		public Action OnDeactivate = delegate { };
		public Action OnPause = delegate { };
		public Action OnUnpause = delegate { };
        #endregion

        #region Inits
        void Awake()
		{
			internalTreeSet = false;

			//Start running agent if flagged
			_active = playOnStart;

			SelectTreeToUse();

			//Initiate and pass in ref to this agent (regardless of active status)
			_internalTree?.RunInitiate(this);
		}
		void OnEnable()
		{
			//Register events
			OnActivate += (ActivateCallback);
			OnDeactivate += (DeactivateCallback);
			OnPause += (PauseCallback);
			OnUnpause += (UnpauseCallback);

			//Start internal ticker
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
				else
				{
					//Finally, internal tree is available for use
					//No more null checks required for internal tree
					internalTreeSet = true;
				}
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
				if (BhaveManager.singleton.tickMode == TickMode.Fixed && _active)
				{
					//Entering Fixed Tick Mode

					//Handle pause state
					if (_paused)
						PauseTick(_paused);
					else
						FullTick();

					//Stop agent if restart flag false
					if (!restartWhenDone)
						_active = false;

					yield return new WaitForSeconds(BhaveManager.singleton.tickRate);
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
			if (BhaveManager.singleton.tickMode != TickMode.EveryFrame || !_active)
				return;

			//Handle pause state
			if (_paused)
				PauseTick(_paused);
			else
				FullTick();

			//Stop agent if restart flag false
			if (!restartWhenDone)
				_active = false;
		}
		//----------------- TickMode.Manual ----------------
		//User can access this method through BhaveManager.Tick() (Cleaner API)
		/// <summary>
		/// Runs core BhaveTree execution stack (Begin, Execute, End)
		/// </summary>
		internal void FullTick()
		{
			_internalTree?.RunBegin();
			if (_internalTree) agentStatus = _internalTree.RunExecute(this);
			_internalTree?.RunEnd();

			HandleSystemNodeStates();
		}
		/// <summary>
		/// Runs core BhaveTree pause execution
		/// </summary>
		internal void PauseTick(bool paused)
		{
            _internalTree?.RunPause(paused);

			HandleSystemNodeStates();
		}


		void HandleSystemNodeStates()
		{
			switch (agentStatus)
			{
				case NodeState.None:	//Failsafe: Shut everything down anyways. Should be in editor
				case NodeState.Aborted:
					SetActive(false);	//Shut down immediately
					break;
				case NodeState.Suspended:
					SetPaused(true);	//Pause immediately
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
			OnUnpause -= UnpauseCallback;
			OnPause -= PauseCallback;
			OnDeactivate -= DeactivateCallback;
			OnActivate -= ActivateCallback;

			//Restore tree (should be ok to overwrite as bhaveTreeModule is separate)
			_internalTree = backupTree;
		}
		void OnDestroy()
		{
			_internalTree?.RunShutdown();
		}
		#endregion

		#region Callbacks
		public void ActivateCallback()
		{
			StartCoroutine(FixedTickRoutine());
			//If pauseIfDeactivated set to true then user will have to manually unpause upon reactivation
		}
		public void DeactivateCallback()
		{
			if (pauseIfDeactivated)
			{
				_paused = true;
			}
			else
			{
				StopCoroutine(FixedTickRoutine());
			}
		}
		public void PauseCallback() { }
		public void UnpauseCallback() { }
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
