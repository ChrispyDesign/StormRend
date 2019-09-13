using UnityEngine;
using BhaVE.Core;
using BhaVE.Delegates;
#if UNITY_EDITOR
using UnityEditor;
using BhaVE.Editor;
#endif

namespace BhaVE.Nodes.Leafs
{
	public class Action : Leaf
	{
		//Delegate
		[SerializeField] BhaveAction deleg;
		public void SetDelegate(BhaveAction action) { deleg = action; }     //API feels more official

		//State
		//- Actions should generally return success.
		//- But for whatever reason could be overriden by the user
		[SerializeField] bool overrideOn = false;
		[SerializeField] NodeState stateOverride = NodeState.Success;

		#region Core Methods
		protected internal override void OnAwaken(BhaveAgent agent) => deleg?.Awaken(agent);
		protected internal override void OnInitiate(BhaveAgent agent) => deleg?.Initiate(agent);
		protected internal override void OnBegin() => deleg?.Begin();
		protected internal override NodeState OnExecute(BhaveAgent agent)
		{
			switch (deleg?.Execute(agent))
			{
#if UNITY_EDITOR   //For BHEditor live view and debugging
				case NodeState.None:
					state = NodeState.None;
					break;
#endif
				case NodeState.Failure:
					state = NodeState.Failure;
					break;
				case NodeState.Success:
					state = NodeState.Success;
					break;
				case NodeState.Pending:
					state = NodeState.Pending;
					break;

				case null: throw new System.NullReferenceException("Action delegate is null!");
				default: throw new System.NotImplementedException("NodeState not implemented yet");
			}
			//Override
			if (overrideOn) state = stateOverride;

			return state;
		}
		protected internal override void OnPause(bool paused) => deleg?.Paused(paused);
		protected internal override void OnEnd() => deleg?.End();
		protected internal override void OnShutdown() => deleg?.Shutdown();
		#endregion

#if UNITY_EDITOR
		protected override void OnDestroy()
		{
			base.OnDestroy();

			//Make sure to remove delegate from tree and delete
			if (deleg)
			{
				AssetDatabase.RemoveObjectFromAsset(deleg);
				DestroyImmediate(deleg);
			}
		}

		BhaveAction interimDeleg;
		bool delegateChanged = false;   //Required to prevent excessive autosaving

		void OnEnable()
		{
			//Prevent delegates from being cleared upon editor restart
			if (deleg) interimDeleg = deleg;
		}

		public override void DrawContent()
		{
			// eData.name = "Action";
			GUILayout.Label("!", BhaVEditor.skin.label);
			// base.DrawContent();

			EditorGUI.BeginChangeCheck();

			interimDeleg = EditorGUILayout.ObjectField(interimDeleg, typeof(BhaveAction), false, GUILayout.MaxHeight(13)) as BhaveAction;

			//If this is true then something has definitely changed
			//Delegate field has changed OR original delegate object was deleted
			if (EditorGUI.EndChangeCheck() || !interimDeleg && !delegateChanged)
			{
				//Make way for change
				if (deleg) AssetDatabase.RemoveObjectFromAsset(deleg);

				//Remove delegate
				if (!interimDeleg)
				{
					Debug.Log(this.name + "'s delegate cleared");

					delegateChanged = true;
					deleg = interimDeleg = null;
				}
				//Change delegate
				else if (interimDeleg != deleg)
				{
					delegateChanged = true;
					deleg = interimDeleg = Instantiate(interimDeleg);

					if (!BhaVEditor.instance.currentProjectIsAgent)
					{
						//If current project is bhaveTree module then add asset to it
						AssetDatabase.AddObjectToAsset(deleg, base.eData.owner);
					}

					Debug.LogFormat("{0} action delegate added to {1}",
						deleg.name, this.name);
				}

				//Save changes
				EditorUtility.SetDirty(base.eData.owner);
				BhaVEditor.Autosave();
			}
		}
#endif
	}


	#region Experimental
	//Action that takes in one extra custom parameter
	// public class Action<T> : Leaf
	// {
	// 	//This needs to be set in BhaVEditor
	// 	public BhaveAction<T> @delegate { get; private set; }

	// 	public void SetDelegate(BhaveAction<T> del) { @delegate = del; }

	// 	/// <summary>
	// 	/// How can this be set in the bhaveditor?
	// 	/// </summary>
	// 	public T param1 { get; set; }


	// 	[Tooltip("Custom return type")]
	// 	[SerializeField]
	// 	protected NodeState returnState = NodeState.Success;   //Should generally return success

	// 	protected internal override NodeState OnExecute(BhaveAgent agent)
	// 	{
	// 		if (@delegate)
	// 			@delegate.Execute(agent, param1);
	// 		else
	// 			Debug.Log("No delegate object assigned!");

	// 		return returnState;
	// 	}
	// }
	#endregion
}
