using UnityEngine;
using UnityEditor;
using BhaVE.Core;
using BhaVE.Delegates;
using BhaVE.Editor;

namespace BhaVE.Nodes.Leafs
{
	// [CreateAssetMenu(menuName = "BhaVEInternal/Leafs/Condition")]
	public class Condition : Leaf
	{
		//Delegate object
		[SerializeField] BhaveCondition deleg;
		public void SetDelegate(BhaveCondition del) { this.deleg = del; }

		#region Core Methods
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

				case null: throw new System.NullReferenceException("Condition delegate is null!");
				default: throw new System.NotImplementedException("NodeState not implemented yet");
			}
			return state;
		}
		protected internal override void OnPause(bool paused) => deleg?.Paused(paused);
		protected internal override void OnEnd() => deleg?.End();
		protected internal override void OnShutdown() => deleg?.Shutdown();

		protected override void OnDestroy()
		{
			//Make sure to remove delegates from tree and the delete
			if (deleg)
			{
				AssetDatabase.RemoveObjectFromAsset(deleg);
				DestroyImmediate(deleg);
			}

			base.OnDestroy();
		}
		#endregion

#if UNITY_EDITOR
		BhaveCondition interimDeleg;
		bool delegateChanged = false;   //Required to prevent excessive autosaving

		void OnEnable()
		{
			//Prevent delegates from being cleared upon editor restart
			if (deleg) interimDeleg = deleg;
		}

		public override void DrawContent()
		{
			eData.name = "Condition";
			GUILayout.Label("?", BhaVEditor.skin.label);
			base.DrawContent();

			EditorGUI.BeginChangeCheck();

			interimDeleg = EditorGUILayout.ObjectField(interimDeleg, typeof(BhaveCondition), false) as BhaveCondition;

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

					Debug.LogFormat("{0} condition delegate added to {1}",
						deleg.name, this.name);
				}

				//Save changes
				EditorUtility.SetDirty(base.eData.owner);
				BhaVEditor.Autosave();
			}
		}
#endif
	}
}