using UnityEditor;
using BhaVE.Nodes;
using BhaVE.Core;
using BhaVE.Nodes.Composites;
using UnityEngine;
using BhaVE.Nodes.Decorators;
using BhaVE.Nodes.Leafs;
using BhaVE.Editor.Settings;
using BhaVE.Editor.Enums;

namespace BhaVE.Editor
{
#if UNITY_EDITOR

	//Note: This should really be in an Editor folder in it's own assembly
	public partial class BhaVEditor : EditorWindow
	{
		#region Core
		//Constants
		public static readonly Vector2 minWindowSize = new Vector2(320, 400);

		//Settings
		public static GUISkin skin;
		public static BHESettings settings;
		public static BhaVEditor instance => GetWindow<BhaVEditor>();   //Cheap singleton


		[MenuItem("BhaVE/Editor")]
		static void CallEditorWindow()
		{
			BhaVEditor editor = GetWindow<BhaVEditor>();
			editor.titleContent = new GUIContent("BhaVE");
			editor.minSize = minWindowSize;
			editor.wantsMouseMove = true;
			editor.wantsMouseEnterLeaveWindow = false;
		}

		void OnEnable()
		{
			untitledIdx = 1;
			//hideFlags = HideFlags.None;

			//Core
			// AssetDatabase.lo
			settings = Resources.Load("BHSettings") as BHESettings;
			skin = Resources.Load("BHEditorSkin") as GUISkin;

			//Register events
			EditorApplication.playModeStateChanged += OnEditorStateChange;
			Undo.undoRedoPerformed += OnUndoRedo;
		}

		/// <summary> Unity callback thatd updates this window; Akin to MonoBehaviour.Update(); </summary>
		void OnGUI()
		{
			DrawBackground();
			DisplayDebugs();

			//Don't draw if there's no active project
			if (!activeTree)
			{
				var center = new Rect(0, 0, position.width, position.height);
				GUI.Label(center, "No Project", skin.label);
				return;
			}


			ProcessEvents();
			DrawContent();
			DrawNodes();

			Repaint();
			// activeTreeSO.Update();
			/// TO BE CONTINUED!!
			/// Thoughts: Trying to use serialized objects and properties instead to get the benefit of inbuilt undos etc.
			/// Instead of having to mess aroudn with set dirty
			// activeTreeSO.ApplyModifiedProperties();
		}

		void OnDisable()
		{
			//Unregister events
			EditorApplication.playModeStateChanged -= OnEditorStateChange;
			Undo.undoRedoPerformed -= OnUndoRedo;
		}
		#endregion

		#region Draws
		Vector2 gridOffset = Vector2.zero;
		Vector2 gridDrag = Vector2.zero;

		void DrawBackground()
		{
			Vector2 border = new Vector2(2, 23);
			Rect bgrect = new Rect(border.x, border.y, position.width - border.x * 2, position.height - border.y * 2 + 20);

			Handles.BeginGUI();

			Handles.DrawSolidRectangleWithOutline(bgrect, settings.editorBGColour, Color.black);

			Handles.EndGUI();
		}

		/// <summary>
		/// Draw all main contents of the window including nodes
		/// </summary>
		void DrawContent()
		{
			//Get last rect
			// Rect contentRect = GUILayoutUtility.GetLastRect();
			// contentRect.y -= Screen.height;
			// contentRect.height = Screen.height;

			// float border = 20;
			Vector2 border = new Vector2(2, 23);
			Rect contentRect = new Rect(border.x, border.y, position.width - border.x * 2f, position.height - border.y * 2f + 20);

			GUI.BeginScrollView(contentRect, Vector2.zero, contentRect);
			// Debug.Log("gridOffset: " + gridOffset);

			//Draw background grid
			DrawGrid(10, Color.black, 0.2f);
			DrawGrid(100, Color.black, 0.4f);

			//Draw nodes
			BeginWindows();
			DrawNodes();
			EndWindows();

			GUI.EndScrollView();
		}

		/// <summary>
		/// Draws a grid using Handles
		/// </summary>
		void DrawGrid(float spacing, Color colour, float transparency)
		{
			float dragFineTune = 0.5f;

			int horizontalDivisions = Mathf.CeilToInt(Screen.width / spacing);
			int verticalDivisions = Mathf.CeilToInt(Screen.height / spacing);

			Handles.BeginGUI();

			//Set color and transparency
			Handles.color = new Color(colour.r, colour.g, colour.b, transparency);

			//Offset the grid when all nodes are dragged
			gridOffset += gridDrag * dragFineTune;

			//Calculate new offset
			Vector3 newOffset = new Vector3(gridOffset.x % spacing, gridOffset.y % spacing);

			//Draw vertical lines
			for (int i = 0; i <= horizontalDivisions; i++)
			{
				Handles.DrawLine(new Vector3(spacing * i, -spacing) + newOffset, new Vector3(spacing * i, Screen.height + spacing) + newOffset);
			}

			//Horizontals
			for (int i = 0; i <= verticalDivisions; i++)
				Handles.DrawLine(new Vector3(-spacing, spacing * i) + newOffset, new Vector3(Screen.width + spacing, spacing * i) + newOffset);

			Handles.EndGUI();
		}

		void DrawNodes()
		{
			//Draw connections for each node first so that they're underneath the nodes?
			foreach (Node n in activeTree.eNodes)
			{
				if (n != null)
					n.DrawConnections();
			}

			//Draw each node itself as a window using "GUI.Window()"
			for (int i = 0; i < activeTree.eNodes.Count; i++)
			{
				GUI.color = GetNodeColour(activeTree.eNodes[i]);

				activeTree.eNodes[i].eData.rect = activeTree.eNodes[i].DrawNode(i);

				#region Experimental
				//This adjusts the node automatically to fit it's contents
				// nodes[i].rect = GUILayout.Window(i, nodes[i].rect, DrawNodeGUI, nodes[i].title, skin.GetStyle("Window"));
				//This draws the node as a texture
				// GUI.Box(activeTree.eNodes[i].eData.rect, "Box");
				#endregion

				//Reset GUI color
				GUI.color = settings.sequenceColour;
			}
		}

		//Return correct draw color for specified node based on it's type
		Color GetNodeColour(Node n)
		{
			switch (n.state)
			{
				case NodeState.Aborted:
					return settings.abortColor;
				case NodeState.Suspended:
					return settings.pauseColor;

				case NodeState.None:    //If not in play mode
					if (n == activeTree.root) return settings.rootColour;
					if (n is Selector) return settings.selectorColour;
					if (n is Sequence) return settings.sequenceColour;
					if (n is Decorator) return settings.decoratorColour;
					if (n is Condition) return settings.conditionColour;
					if (n is Action) return settings.actionColor;
					if (n is Deactivator) return settings.deactivateColor;
					break;

				//Play mode
				case NodeState.Pending:
					return settings.pendingColor;
				case NodeState.Failure:
					return settings.failureColor;
				case NodeState.Success:
					return settings.successColour;
			}
			return Color.magenta;       //Default
		}

		/// <summary>
		/// Moves all nodes to simulate panning the view
		/// </summary>
		/// <param name="delta"></param>
		void PanView(Vector2 delta)
		{
			foreach (var n in activeTree.eNodes)
			{
				n.eData.rect.position += delta;
			}
			// Repaint();
		}
		#endregion

		#region Input
		private enum DialogChoice
		{
			Yes = 0,
			Cancel = 1,
			No = 2,
		}
		BHEditorAssist ea;

		//Handle mouse and keyboard events in editor
		void ProcessEvents()
		{
			//Set Assistant
			ea.SetEvent(Event.current);
			//----------- Reset flags ---------------
			ea.isOverNode = false;
			gridDrag = Vector2.zero;
			//----------- Mouse Events ---------------
			foreach (var n in activeTree.eNodes)
			{
				//If mouse inside node...
				if (n.eData.rect.Contains(ea.mousePosition))
				{
					ea.isOverNode = true;
					if (ea.mouseClicked)
					{
						if (ea.leftClick)
						{
							if (ea.isConnecting)
							{
								//END CONNECTION
								EndConnection(n);
							}
							else
							{
								//SELECT + DRAG
								ea.targetNode = n;
								ea.targetNode.eData.isDraggable = true;
							}

							if (ea.doubleClick)
							{
								//SHOW ON INSPECTOR
								Selection.activeObject = ea.targetNode as Object;
							}
						}
						else if (ea.rightClick)
						{
							//EDIT NODE CONTEXT
							ea.targetNode = n;
							CallEditNodeContext(ea.now);
						}
						else if (ea.middleClick)
						{
							//SELECT ONLY
							ea.targetNode = n;
						}
						break;
					}
					else if (ea.mouseReleased)
					{
						if (ea.targetNode) ea.targetNode.eData.isDraggable = false;

						if (ea.leftClick && ea.isConnecting)
						{
							//END CONNECTION
						}
						break;
					}
					break;
				}
			}
			//Else mouse was outside of node
			if (!ea.isOverNode)
			{
				if (ea.mouseClicked)
				{
					if (ea.leftClick)
					{
						//Show add node context menu if 
						if (ea.isConnecting)
						{
							//ADD NODE CONTEXT
							CallAddNodeContext(ea.now);
						}
						else
						{
							ea.isConnecting = false;
							ea.targetNode = null;
						}
					}
					else if (ea.rightClick)
					{
						ea.isConnecting = false;
						ea.targetNode = null;

						//ADD NODE CONTEXT
						CallAddNodeContext(ea.now);
					}
					else if (ea.middleClick)
					{
						ea.isConnecting = false;
						ea.targetNode = null;
					}
				}
				else if (ea.mouseDragged)
				{
					if (ea.leftClick)
					{
						if (ea.altKey)
						{
							//PAN VIEW
							PanView(ea.delta);
							gridDrag = ea.delta;
						}
					}
					else if (ea.middleClick)
					{
						//PAN VIEW
						PanView(ea.delta);
						gridDrag = ea.delta;
					}
				}
				else if (ea.mouseReleased)
				{
					if (ea.isConnecting)
					{
						//ADD NODE CONTEXT
						//(Only if implementing drag from node to create a connection)
						// ShowAddNodeContext(e);
					}
				}
			}
			if (ea.isConnecting)
			{
				//CONTINUE CONNECTION
				ContinueConnection(ea.connStartPos, ea.mousePosition);
			}
			//----------- Keyboard Events -------------
			if (ea.keyDown)
			{
				switch (ea.keyCode)
				{
					case KeyCode.Escape:
						ea.isConnecting = false;
						ea.targetNode = null;
						break;
					case KeyCode.Delete:
						DeleteNode(ea.targetNode, true);
						ea.Clear();
						break;
				}
			}
		}
		#endregion

		#region Callbacks
		void OnUndoRedo()
		{
			Repaint();
		}
		void OnEditorStateChange(PlayModeStateChange stateChange)
		{
			switch (stateChange)
			{
				// case PlayModeStateChange.ExitingEditMode:
				case PlayModeStateChange.EnteredPlayMode:
					//If the current active tree is being used in scene...
					//Initialize all nodes in active tree to failure
					SetAllNodeStates(NodeState.Failure);
					break;

				// case PlayModeStateChange.ExitingPlayMode:
				case PlayModeStateChange.EnteredEditMode:
					//Reset all active tree node states to none
					SetAllNodeStates(NodeState.None);
					break;
			}
		}

		void CallAddNodeContext(Event e)
		{
			GenericMenu m = new GenericMenu();
			m.AddItem(new GUIContent("Composites/"), false, null);
			m.AddItem(new GUIContent("Composites/Selector"), false, OnAddNodeContext, BHECommands.AddSelector);
			m.AddItem(new GUIContent("Composites/Sequence"), false, OnAddNodeContext, BHECommands.AddSequence);

			m.AddItem(new GUIContent("Decorators/"), false, null);
			m.AddItem(new GUIContent("Decorators/Inverter"), false, OnAddNodeContext, BHECommands.AddInverter);
			m.AddItem(new GUIContent("Decorators/Succeeder"), false, OnAddNodeContext, BHECommands.AddSucceeder);
			m.AddItem(new GUIContent("Decorators/Repeater"), false, OnAddNodeContext, BHECommands.AddRepeater);

			m.AddItem(new GUIContent("Leafs/"), false, null);
			m.AddItem(new GUIContent("Leafs/Condition"), false, OnAddNodeContext, BHECommands.AddCondition);
			m.AddItem(new GUIContent("Leafs/Action"), false, OnAddNodeContext, BHECommands.AddAction);
			m.AddItem(new GUIContent("Leafs/Deactivator"), false, OnAddNodeContext, BHECommands.AddDeactivator);

			m.ShowAsContext();
			e.Use();    //Finish using this event
		}

		void CallEditNodeContext(Event e)
		{
			GenericMenu m = new GenericMenu();
			if (CanStartConnection(ea.targetNode)) m.AddItem(new GUIContent("Create Connection"), false, OnEditNodeContext, BHECommands.CreateConnection);
			else m.AddDisabledItem(new GUIContent("Create Connection"));
			m.AddSeparator("");
			m.AddItem(new GUIContent("Replace this Node"), false, OnEditNodeContext, BHECommands.ReplaceNode);
			m.AddItem(new GUIContent("Set as Root Node"), false, OnEditNodeContext, BHECommands.SetAsRoot);
			m.AddItem(new GUIContent("Show on Inspector"), false, OnEditNodeContext, BHECommands.ShowOnInspector);
			m.AddSeparator("");
			m.AddItem(new GUIContent("Delete Node"), false, OnEditNodeContext, BHECommands.DeleteNode);

			m.ShowAsContext();
			e.Use();    //Finish using this event
		}

		void OnAddNodeContext(object o)
		{
			//Assume a new node will be created
			Node newNode = null;

			BHECommands a = (BHECommands)o;
			switch (a)
			{
				//CREATION
				case BHECommands.AddSequence:
					newNode = CreateInstance<Sequence>();
					break;
				case BHECommands.AddSelector:
					newNode = CreateInstance<Selector>();
					break;
				case BHECommands.AddInverter:
					newNode = CreateInstance<Inverter>();
					break;
				case BHECommands.AddSucceeder:
					newNode = CreateInstance<Succeeder>();
					break;
				case BHECommands.AddRepeater:
					newNode = CreateInstance<Repeater>();
					break;
				case BHECommands.AddAction:
					newNode = CreateInstance<Action>();
					break;
				case BHECommands.AddCondition:
					newNode = CreateInstance<Condition>();
					break;
				case BHECommands.AddDeactivator:
					newNode = CreateInstance<Deactivator>();
					break;
				default: Debug.Log("Nothing created"); break;
			}

			//Handle if node is to be created
			if (newNode != null)
			{
				newNode.eData.rect = new Rect(ea.mousePosition.x - settings.nodeSize.x * 0.5f, ea.mousePosition.y, settings.nodeSize.x, settings.nodeSize.y);
				AddNode(newNode);

				//Also child this new node if in connecting mode
				if (ea.isConnecting)
				{
					EndConnection(newNode);
				}
			}
		}

		void OnEditNodeContext(object o)
		{
			BHECommands a = (BHECommands)o;
			try
			{
				if (!ea.targetNode) throw new System.NullReferenceException("Subject node is null!");

				switch (a)
				{
					case BHECommands.CreateConnection:
						StartConnection(ea.targetNode);
						break;

					case BHECommands.SetAsRoot:
						activeTree.root = ea.targetNode;
						ea.Clear();
						break;

					case BHECommands.DeleteNode:
						DeleteNode(ea.targetNode);
						ea.Clear();
						break;

					case BHECommands.ReplaceNode:
						throw new System.NotImplementedException("Replace Node not implemented yet");

					case BHECommands.ShowOnInspector:
						if (ea.targetNode) Selection.activeObject = ea.targetNode as Object;
						break;

					default: throw new System.InvalidOperationException("Invalid command!");
				}
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e.Message);
			}
		}

		void OnSelectionChange()
		{
			var selection = Selection.activeObject;

			//BhaveAgent
			if (selection is GameObject)
			{
				var selectedAgent = (selection as GameObject).GetComponentInChildren<BhaveAgent>();
				if (selectedAgent)
				{
					Debug.Log("BhaveAgent Selected");

					// //Ignore if it's the same
					// if (activeAgent == selectedAgent)
					// {
					// 	Debug.Log("<Same Project>");
					// 	return;
					// }

					//Confirm load
					if (promptOnLoad)
					{
						var msg = string.Format("Save and load in Agent {0}?", selectedAgent.name);
						if (DialogPrompt("Load Agent?", msg) == DialogChoice.Cancel)
						{
							return;
						}
					}
					//Save either way
					else
					{
						Save(false);    //Don't show save nofication since it'll conflict
					}

					//Set active project
					activeAgent = selectedAgent;
					activeTree = selectedAgent.internalTree;
					SetAllNodeStates(NodeState.None);

					//Reset flags
					projectIsDirty = false;
					projectHasBeenSaved = false;

					ShowNotification(new GUIContent(string.Format("{0} Loaded ", activeAgent.name)));
				}
			}
			//BhaveTree Module
			else if (selection is BhaveTree)
			{
				var selectedBhaveTreeModule = selection as BhaveTree;

				Debug.Log("BhaveTree Module Selected");

				// if (activeTree == selectedBhaveTreeModule)
				// {
				// 	Debug.Log("<Same Project>");
				// 	return;
				// }

				if (projectIsDirty)
				{
					if (promptOnLoad)
					{
						var msg = string.Format("Save and load in Module {0}?", selectedBhaveTreeModule.name);
						if (DialogPrompt("Load Module?", msg) == DialogChoice.Cancel)
						{
							return;
						}
					}
					else
					{
						Save(false);
					}
				}

				//Set active project
				activeAgent = null;
				activeTree = selectedBhaveTreeModule;
				SetAllNodeStates(NodeState.None);

				//Reset flags
				projectIsDirty = false;
				projectHasBeenSaved = false;

				ShowNotification(new GUIContent(string.Format("{0} Loaded ", activeTree.name.ToString())));
			}
		}
		#endregion

		#region File
		int untitledIdx = 1;
		BhaveAgent activeAgent;
		BhaveTree activeTree;   //The main bhave tree that this editor is currently working on
		static string moduleAssetPath = "Assets/";
		public void SetActiveAgent(BhaveAgent agent) => activeAgent = agent;
		public void SetActiveTree(BhaveTree tree) => activeTree = tree;
		public bool currentProjectIsAgent => activeAgent != null;

		//Flags
		static bool autosave;
		static bool projectIsDirty = false;     //Needs to be static for external autosave
		bool projectHasBeenSaved = false;
		bool promptOnLoad = true;
		public void SetProjectDirty() => projectIsDirty = true;

		/// <summary>
		/// Creates a new external BhaveTree module?
		/// </summary>
		void New()
		{
			/* Scenarios:
			1. If agent is open
				if project dirty then Prompt user to overwrite agent's internal tree
				null and create new tree on agent's internal
			2. If module is open
				if project dirty then prompt user to overwrite module
				null module tree
				create new tree
			*/
			try
			{
				//If Agent selected then create a new tree on the agent
				if (activeAgent)
				{
					//Allow user to cancel
					if (projectIsDirty)
					{

						if (DialogPrompt("New Project",
							string.Format("Overwrite {0} internal tree?", activeAgent.name),
							false) == DialogChoice.Cancel)  //Don't save because we'll be overwriting anyways
						{
							return;
						}
					}

					//Overwrite (accessing Agent's internal scope)
					activeAgent._internalTree = null;   //Clear
					activeAgent._internalTree = CreateInstance<BhaveTree>();
					activeAgent._internalTree.name = activeAgent.name + ".internal";

					//Set actives
					activeTree = activeAgent.internalTree;

					//Reset flags
					projectHasBeenSaved = false;    //Wait... has it been saved?
					projectIsDirty = false;
				}
				//else (active project is a BhaveTree module) create a new bhavetree module (ideally in memory)
				else if (activeTree)
				{
					//Confirm if there are nodes
					if (activeTree?.eNodes.Count > 0)
					{
						if (DialogPrompt("New Project",
							string.Format("Create new module {0}?", activeTree.name),
							false) == DialogChoice.Cancel)
						{
							return;
						}
					}

					//Clear the current tree (Don't need to create a new instance of BhaveTree)
					activeTree.Clear();

					//Reset flags
					projectHasBeenSaved = false;    //Wait... has it been saved?
					projectIsDirty = false;
				}
				//No project open, create new external BhaveTree module
				else
				{
					activeTree = CreateInstance<BhaveTree>();
					activeTree.name = "Untitled Module " + untitledIdx++;

					//Prompt user where to put
					moduleAssetPath = EditorUtility.SaveFilePanelInProject("Save Module As", activeTree.name,
							"asset", "message", moduleAssetPath);
					AssetDatabase.CreateAsset(activeTree, moduleAssetPath);

					//Reset flags
					projectHasBeenSaved = false;    //Wait... has it been saved?
					projectIsDirty = false;
				}
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e.Message);
			}
		}

		/// <summary>
		/// Loads in a new external BhaveTree module?
		/// </summary>
		void Load(Object project)
		{
			try
			{
				if (projectIsDirty)
				{
					if (DialogPrompt("Load Project", "Save changes before loading?") == DialogChoice.Cancel)  //User cancels operation
						return;
				}

				LoadProject();
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e.Message);
			}

			void LoadProject()
			{
				//IF working on an Agent


				//ELSE IF working on an external tree module


				projectHasBeenSaved = false;

				throw new System.NotFiniteNumberException();
			}
		}

		/// <summary>
		/// Saves the current work and shows notification
		/// </summary>
		void Save(bool showNofication = true)
		{
			//THOUGHTS
			// - Maybe SetDirty() should be run right before running SaveAssets()
			Debug.Log("Saving...");
			try
			{
				//BhaveAgent
				if (activeAgent)
				{
					throw new System.NotImplementedException("Agent tree saving not implemented!");

					if (showNofication)
						ShowNotification(new GUIContent(string.Format("{0} saved!", activeAgent.name)));

					//Aren't agents already saved?

					//Reset flags
					projectIsDirty = false;
					projectHasBeenSaved = true;
				}
				//BhaveTree Module
				else if (activeTree)
				{
					if (showNofication)
						ShowNotification(new GUIContent(string.Format("{0} saved!", activeTree.name)));

					//THE ACTUAL SAVE
					if (projectIsDirty) EditorUtility.SetDirty(activeTree);     //Seems like you almost don't have to do this
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();

					//Reset flags
					projectIsDirty = false;
					projectHasBeenSaved = true;
				}
				else
				{
					throw new System.Exception("No current project");
				}
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e.Message);
			}
		}
		void SaveAs()
		{
			/* 
			GeneralSave()
				if project is BhaveAgent
					if this project is not dirty AND has not been saved yet
						Save() to agent
				else if project BhaveTree module
					if this project is not dirty And has not been saved yet
						Save() to assetdatabase

			Save()
				if project is dirty
					if project has not been saved yet
						SaveAs()
					else 
						if (activeAgent)
							[How do you save an agent's internal tree? Wouldn't they already be saved?]
						else if (activeTree)
							AssetDatabase.SetDirty(activeAgent)


			SaveAs()
				if project is Agent
					????
				else if project is TreeModule
					SetDirty()
					SaveAssets()
					Refresh()
					


			//1. Isn't on an agent
			//2. Doesn't have a 
			*/
		}

		/// <summary>
		/// Unload the current project
		/// </summary>
		void CloseProject()
		{
			//Prompt to save first
			if (projectIsDirty)
			{
				DialogPrompt("Close Project", "Save changes before closing?");
			}

			//Nulling these don't delete the actual objects themselves
			activeAgent = null;
			activeTree = null;

			projectIsDirty = false;
			projectHasBeenSaved = false;
			ea.Clear();
		}

		/// <summary>
		/// Prompt user if they want to go ahead and load the selected object
		/// </summary>
		//UPDATE: Since I can't figure out how to NOT save, we're going with a confirm load instead since the project will be autosaved anyways
		DialogChoice DialogPrompt(string title, string message, bool doSave = true)
		{
			bool choice = EditorUtility.DisplayDialog(title, message, "Yes", "No");

			switch (choice)
			{
				case true:
					if (doSave) Save();
					return DialogChoice.Yes;

				case false:
					return DialogChoice.Cancel;

				default:
					throw new System.InvalidOperationException();
			}
		}

		/// <summary>
		/// Saves current work if editor autosave setting true
		/// </summary>
		public static void Autosave()
		{
			if (autosave)
			{
				Debug.Log("Autosaving...");

				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				projectIsDirty = false;
			}
		}
		#endregion

		#region Connections
		//Returns true if specified node can start a connection
		bool CanStartConnection(Node startNode)
		{
			return (startNode is Composite || startNode is Decorator);
		}

		//Start the process of connecting nodes if the subject node is valid
		void StartConnection(Node startNode)
		{
			//Only composites and decorators can have children
			if (CanStartConnection(startNode))
			{
				ea.isConnecting = true;
				ea.connStartNode = ea.targetNode;

				//Center Bottom of the node
				ea.connStartPos = new Vector2(
					ea.targetNode.eData.rect.x + ea.targetNode.eData.rect.width * 0.5f,
					ea.targetNode.eData.rect.y + ea.targetNode.eData.rect.height);
			}
			else
			{
				Debug.Log("Cannot start connection. Invalid node type!");
			}
		}

		//Continue drawing the current connection
		void ContinueConnection(Vector2 startPos, Vector2 currentPos)
		{
			BhaVEditor.DrawConnection(startPos, currentPos);
		}

		//End connection mode and connect nodes if valid
		void EndConnection(Node endNode)
		{
			//Connection should end here
			ea.isConnecting = false;

			try
			{
				if (endNode == ea.connStartNode)
					throw new System.InvalidOperationException("Cannot connect to itself!");

				if (ea.connStartNode.ContainsChild(endNode))
					throw new System.InvalidOperationException("Already connected to this child!");

				ConnectNodes(ea.connStartNode, endNode);
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e.Message);
			}
		}

		/// <summary> Connect two nodes together </summary>
		/// <param name="startNode">Parent node</param>
		/// <param name="endNode">Child node</param>
		internal void ConnectNodes(Node startNode, Node endNode)
		{
			Undo.RecordObject(startNode, "Connect Node");

			startNode.AddChild(endNode);
			Debug.LogFormat("Connecting [{0}] > [{1}]", startNode.name, endNode.name);
			ea.Clear();

			//Dirty
			projectIsDirty = true;
			Autosave();
		}

		//Draw a connection using nodes
		internal static void DrawConnection(Node startNode, Node endNode)
		{
			float thicknessFineTune = 0.25f;

			//Start node: Bottom Center
			Vector2 start = new Vector2(
				startNode.eData.rect.x + startNode.eData.rect.width * 0.5f,     //Center
				startNode.eData.rect.y + startNode.eData.rect.height - settings.lineThickness * thicknessFineTune);          //Bottom

			//End node: Top Center
			var end = new Vector2(
				endNode.eData.rect.x + endNode.eData.rect.width * 0.5f,         //Center
				endNode.eData.rect.y + settings.lineThickness * thicknessFineTune);                                    //Top

			//Set color
			Color? connColour;
			switch (endNode.state)
			{
				case NodeState.Pending:
					connColour = settings.runningLineColor;
					break;
				case NodeState.Success:
					connColour = settings.runningLineColor;
					break;
				default: connColour = settings.defaultLineColour; break;
			}

			//Draw
			DrawConnection(start, end, connColour);
		}

		//Draw a connection using raw positions
		internal static void DrawConnection(Vector2 startPos, Vector2 endPos, Color? color = null)
		{
			var finalColor = color ?? settings.defaultLineColour;

			switch (settings.connectionStyle)
			{
				default:
				case BHEConnectionStyle.Square:
				case BHEConnectionStyle.Linear:
					Handles.DrawBezier(startPos, endPos, startPos, endPos, finalColor, null, settings.lineThickness);
					break;
				case BHEConnectionStyle.Bezier:
					Vector2 startTan = startPos - Vector2.down * settings.bezierTangent;
					Vector2 endTan = endPos - Vector2.up * settings.bezierTangent;
					Handles.DrawBezier(startPos, endPos, startTan, endTan, finalColor, null, settings.lineThickness);
					break;
			}
		}
		#endregion

		#region Nodes
		//Add a new node to the currently active tree
		void AddNode(Node n)
		{
			ReassignRootNode(n);

			//Set
			n.eData.SetOwner(activeTree);
			n.ID = activeTree.eNodes.Count;
			n.name = string.Format("{0}:{1}", n.GetType().Name, n.ID);

			//Add
			Undo.RecordObject(activeTree, "Create " + n.GetType().Name + " Node");  //Register the state of the entire tree before it is changed
			activeTree.eNodes.Add(n);       //Island node
			if (!activeAgent) AssetDatabase.AddObjectToAsset(n, activeTree);      //Attach to module
			Undo.RegisterCreatedObjectUndo(n, "Create " + n.GetType().Name + " Node");  //Register newly created object

			//Save
			projectIsDirty = true;
			Autosave();

			//Log
			Debug.LogFormat("Creating [{0}]", n.name);
		}

		//Delete specified node from currently active tree and disconnect any linked nodes
		void DeleteNode(Node n, bool showPrompt = false)
		{
			if (!n)
			{
				Debug.LogWarningFormat("Cannot delete null node ({0})", n);
				return;
			}

			//Confirm delete from user
			if (showPrompt && DeletePrompt() == DialogChoice.No)
				return;

			//About to delete. Undo register
			Undo.RecordObject(activeTree, "Delete Node");

			if (activeAgent)
			{
				//Does AssetDatabase.RemoveObjectFromAsset work with objects on monobehaviours?
				//Destroyimmediate() below should automatically handle this right?
			}
			//If bhavetree module then need to first remove from BhaveTree module
			else
			{
				AssetDatabase.RemoveObjectFromAsset(n);
			}

			//Delete
			//This also automatically removes any connections or references from related nodes
			activeTree.eNodes.Remove(n);

			// DestroyImmediate(n);
			Undo.DestroyObjectImmediate(n);

			//Reassign
			ReassignNodeNamesAndIDs();
			ReassignRootNode(n);

			//Save
			projectIsDirty = true;
			Autosave();

			DialogChoice DeletePrompt()
			{
				bool choice = EditorUtility.DisplayDialog(
					"Delete Node?",
					string.Format("Are you sure you want to delete [{0}]?", n.name),
					"Yes", "No");
				return (choice) ? DialogChoice.Yes : DialogChoice.No;
			}
		}

		/// <summary>
		/// Automatically reassigns the root node
		/// </summary>
		/// <param name="n">The node that is being added or deleted</param>
		void ReassignRootNode(Node n)
		{
			//If there is only one node in the entire tree (Adding node)
			if (activeTree.eNodes.Count == 0)
			{
				//Make that the root node
				activeTree.root = n;
			}
			//Otherwise if there's more than one node (Deleting node)
			else if (activeTree.eNodes.Count > 0)
			{
				//and if the node in question was the root node
				if (n == activeTree.root)
				{
					//Set the new root node of the current active tree
					activeTree.root = activeTree.eNodes[0];
				}
			}
			else
			{
				throw new System.InvalidOperationException("Invalid node count");
			}
		}

		/// <summary>
		/// Automatically reassigns each node's name and ID
		/// </summary>
		void ReassignNodeNamesAndIDs()
		{
			//Reassign each node's id and name
			int i = 0;
			foreach (var en in activeTree.eNodes)
			{
				en.ID = i;
				en.name = string.Format("{0}:{1}", en.GetType().Name, en.ID);
				i++;
			}
		}

		/// <summary>
		/// Set all node states of current tree
		/// </summary>
		void SetAllNodeStates(NodeState state)
		{
			if (activeTree)
				foreach (var en in activeTree.eNodes)
					en.state = state;
		}
		#endregion

		#region Styling

		#endregion

		#region Debugs
		void DisplayDebugs()
		{
			try
			{
				// GUILayout.BeginArea(new Rect(position.x, position.y, width, height));
				GUILayout.BeginHorizontal();
				GUILayout.Label("File");
				promptOnLoad = GUILayout.Toggle(promptOnLoad, "Prompt On Load");
				if (GUILayout.Button("New")) { New(); }
				if (GUILayout.Button("Load")) { throw new System.NotImplementedException(); }
				if (GUILayout.Button("Save")) { Save(); }
				autosave = GUILayout.Toggle(autosave, "AutoSave");
				if (GUILayout.Button("Close")) { CloseProject(); }
				GUILayout.EndHorizontal();

				// GUILayout.BeginHorizontal();
				// if (GUILayout.Button("SetDirty")) { EditorUtility.SetDirty(activeTree); }
				// if (GUILayout.Button("SaveAssets()")) { AssetDatabase.SaveAssets(); }
				// if (GUILayout.Button("UnloadUnusedAssetsImmediate()")) { EditorUtility.UnloadUnusedAssetsImmediate(); }
				// if (GUILayout.Button("Refresh()")) { AssetDatabase.Refresh(); }
				// if (GUILayout.Button("Repaint()")) { Repaint(); }
				// // GUILayout.Label("AssetDatabase");
				// // if (GUILayout.Button("ADB Refresh")) { AssetDatabase.Refresh(); }
				// GUILayout.EndHorizontal();

				if (!activeTree) return;

				GUILayout.Label("Dirty: " + projectIsDirty);
				GUILayout.Label("Saved: " + projectHasBeenSaved);
				GUILayout.Label("activeAgent: " + (activeAgent ? activeAgent.name : "No active agent"));
				GUILayout.Label("activeTree: " + (activeTree ? activeTree.name : "No active tree"));
				// GUILayout.Label("activeTree node count: " + activeTree?.eNodes.Count);

				GUILayout.Label(string.Format("subjectNode: {0}", ea.targetNode?.name));
				GUILayout.Label("isConnecting: " + ea.isConnecting);
				GUILayout.Label("overNode: " + ea.isOverNode);
				GUILayout.Label(string.Format("startNode: {0}", ea.connStartNode?.name));
				GUILayout.Label("startPos: " + ea.connStartPos);

				// GUILayout.Label("mPos: " + ea.mousePosition);
				// GUILayout.Label("mDelta: " + ea.delta);

				// GUILayout.Label("gridOffset: " + gridOffset);
				// GUILayout.Label("gridDrag: " + gridDrag);


				// GUILayout.EndArea();
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e.Message);
			}
		}
		#endregion
	}
#endif
}