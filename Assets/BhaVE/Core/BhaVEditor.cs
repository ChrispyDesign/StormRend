﻿#if UNITY_EDITOR

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

	//Note: This should really be in an Editor folder in it's own assembly
	public class BhaVEditor : EditorWindow
	{
		#region Core
		//Constants
		public static readonly Vector2 minWindowSize = new Vector2(320, 400);

		//Settings
		public static GUISkin skin;
		public static BHEConfig config;
		public static BhaVEditor instance => GetWindow<BhaVEditor>();   //Cheap singleton

		GUIStyle bhaveLabel, bhaveNodes;

		BhaVEditor()
		{
			//Styles
			bhaveLabel = new GUIStyle();
			bhaveNodes = new GUIStyle();
		}

		[MenuItem("BhaVE/Editor")]
		static void GetWindow()
		{
			BhaVEditor editor = GetWindow<BhaVEditor>();
			editor.titleContent = new GUIContent("BhaVE");
			editor.minSize = minWindowSize;
			// editor.wantsMouseMove = true;
			// editor.wantsMouseEnterLeaveWindow = false;
		}

		void OnEnable()
		{
			untitledIdx = 1;

			//Core
			config = AssetDatabase.LoadAssetAtPath<BHEConfig>("Assets/BhaVE/Config/BHEConfig.asset");
			skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/BhaVE/Config/BHESkin.guiskin");
			// config = Resources.Load("BHSettings") as BHEConfig;
			// skin = Resources.Load("BHEditorSkin") as GUISkin;

			//Register events
			EditorApplication.playModeStateChanged += OnEditorStateChange;
			Undo.undoRedoPerformed += OnUndoRedo;
		}

		void InitTestSerializableObjectWorkflow()
		{
			// activeTreeSO = new SerializedObject(activeTree);
			// activeTreeNodeProp = activeTreeSO.FindProperty("root");
			// activeTreeNode = activeTreeNodeProp.objectReferenceValue as Node;
		}

		/// <summary> Unity callback thatd updates this window; Akin to MonoBehaviour.Update(); </summary>
		void OnGUI()
		{
			// DisplayDebugs();
			DrawBackground();

			//Don't draw content if nothing open
			if (activeTree)
			{
				ProcessEvents();
				DrawWorkspace();
				DrawCurrentConnection();
				DrawNodes();
				DrawCurrentProjectInfo();
				Repaint();
			}
			else
			{
				DrawNullProjectInfo();
			}

			//Always draw the toolbar
			DrawToolbar();
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
		Color defaultGUIColor;

		void DrawBackground()
		{
			Vector2 border = new Vector2(2, 20);
			Rect bgrect = new Rect(border.x, border.y, position.width - border.x * 2, position.height - border.y * 2 + 20 - 3);

			Handles.BeginGUI();

			Handles.color = Color.white;
			Handles.DrawSolidRectangleWithOutline(bgrect, config.workspaceColour, Color.Lerp(Color.white, Color.black, 0.75f));

			Handles.EndGUI();
		}

		/// <summary>
		/// Draw all main contents of the window including nodes
		/// </summary>
		void DrawWorkspace()
		{
			Vector2 border = new Vector2(2, 20);
			Rect contentRect = new Rect(border.x, border.y, position.width - border.x * 2f, position.height - border.y * 2f + 20 - 3);

			GUI.BeginScrollView(contentRect, Vector2.zero, contentRect);

			//Draw background grid
			DrawGrid(config.gridMinor, config.gridMinorColour);
			DrawGrid(config.gridMajor, config.gridMajorColour);

			//Draw nodes
			BeginWindows();
			DrawNodes();
			EndWindows();

			GUI.EndScrollView();
		}

		/// <summary>
		/// Has to be drawn after the workspace so that the connections don't appear below the workspace grid
		/// </summary>
		void DrawCurrentConnection()
		{
			if (ea.isConnecting)
			{
				//CONTINUE CONNECTION
				ContinueConnection(ea.connStartPos, ea.mousePosition);
			}
		}

		/// <summary>
		/// Draw each node using each node's drawing methods
		/// </summary>
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
				defaultGUIColor = GUI.color;

				GUI.color = GetAppropriateNodeColour(activeTree.eNodes[i]);

				//All nodes drawn the same size
				Rect nodeRect = activeTree.eNodes[i].eData.rect;
				nodeRect.size = config.nodeSize;

				//Draw node
				activeTree.eNodes[i].eData.rect = activeTree.eNodes[i].DrawNode(i, nodeRect, skin.window);

				//Reset GUI color
				GUI.color = defaultGUIColor;
			}
		}

		/// <summary>
		/// Draw current project info up top
		/// </summary>
		void DrawCurrentProjectInfo()
		{
			string projectInfoText = null;
			if (activeAgent && activeTree)
			{
				projectInfoText = string.Format("Agent: {0}, Tree: {1}",
				activeAgent.name, activeTree.name);
			}
			else if (!activeAgent && activeTree)
			{
				projectInfoText = string.Format("Module: {0}",
					activeTree.name);
			}
			var labelRect = new Rect(3, 12, position.width, 40);
			GUI.Label(labelRect, projectInfoText, skin.label);
		}

		/// <summary>
		/// Draw info text when projects aren't active
		/// </summary>
		void DrawNullProjectInfo()
		{
			float levelheight = 40;
			Rect[] level = new Rect[3];
			for (int i = 0; i < level.Length; i++)
			{
				level[i] = new Rect(0, levelheight * (i - 1), position.width, position.height);
			}

			//Agent
			if (activeAgent)
			{
				GUI.Label(level[0], string.Format("Agent selected: {0}", activeAgent.name), skin.label);
				//No tree
				GUI.Label(level[1], "No internal tree", skin.label);
				GUI.Label(level[2], "Click New to initialize an internal tree in this agent", skin.label);
			}
			else
			{
				GUI.Label(level[1], "No current project", skin.label);
				GUI.Label(level[2], "Select a BhaveAgent or BhaveTree Module to load in", skin.label);
			}
		}

		void DrawToolbar()
		{
			defaultGUIColor = GUI.color;

			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

			//New button
			if (GUILayout.Button("New", EditorStyles.toolbarButton, GUILayout.Width(40))) { New(); }

			//Load button
			if (GUILayout.Button("Load", EditorStyles.toolbarButton, GUILayout.Width(40)))
			{
				// Load();
				Debug.Log("Load not implemented yet!");
			}

			//Save button
			if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(40))) { Save(); }

			//Close project button
			if (GUILayout.Button("Close", EditorStyles.toolbarButton, GUILayout.Width(45))) { CloseProject(); }

			GUILayout.FlexibleSpace();

			//Delete prompt toggle
			if (confirmDelete) GUI.color = Color.Lerp(defaultGUIColor, Color.black, 0.3f);
			if (GUILayout.Button("Confirm Delete", EditorStyles.toolbarButton, GUILayout.Width(90))) { confirmDelete = !confirmDelete; }
			GUI.color = defaultGUIColor;

			//Load Prompt toggle
			if (confirmLoad) GUI.color = Color.Lerp(defaultGUIColor, Color.black, 0.3f);
			if (GUILayout.Button("Confirm Load", EditorStyles.toolbarButton, GUILayout.Width(85))) { confirmLoad = !confirmLoad; }
			GUI.color = defaultGUIColor;

			//Autosave toggle
			if (autosave) GUI.color = Color.Lerp(defaultGUIColor, Color.black, 0.3f);
			if (GUILayout.Button("Autosave", EditorStyles.toolbarButton, GUILayout.Width(60))) { autosave = !autosave; }
			GUI.color = defaultGUIColor;

			//Close window button
			// if (GUILayout.Button("Close", EditorStyles.toolbarButton, GUILayout.Width(40))) { Close(); }

			GUI.color = defaultGUIColor;
			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// Draws a grid using Handles
		/// </summary>
		void DrawGrid(float spacing, Color colour)
		{
			float dragFineTune = 0.5f;

			int horizontalDivisions = Mathf.CeilToInt(Screen.width / spacing);
			int verticalDivisions = Mathf.CeilToInt(Screen.height / spacing);

			Handles.BeginGUI();

			//Set color and transparency
			Handles.color = colour;

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


		//Return correct draw color for specified node based on it's type
		Color GetAppropriateNodeColour(Node n)
		{
			switch (n.state)
			{
				//Play mode colours
				case NodeState.Aborted:
					return config.abortColour;
				case NodeState.Suspended:
					return config.pauseColour;
				case NodeState.Failure:
					return config.failureColor;
				case NodeState.Success:
					return config.successColour;
				case NodeState.Pending:
					return config.pendingColor;

				//Editor mode colours
				case NodeState.None:
					if (n == activeTree.root) return config.rootColour;
					if (n is Selector) return config.selectorColour;
					if (n is Sequence) return config.sequenceColour;
					if (n is Decorator) return config.decoratorColour;
					if (n is Condition) return config.conditionColour;
					if (n is Action) return config.actionColor;
					if (n is Deactivator) return config.deactivateColor;
					if (n is Suspender) return config.suspendColor;
					break;
			}
			return Color.magenta;       //Failsafe
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
			//CHECKS INSIDE THE NODE
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
								// ea.Use();	//Let this propagate down to any delegates that may be on this field
							}
						}
						else if (ea.rightClick)
						{
							//EDIT NODE CONTEXT
							ea.targetNode = n;
							CallEditNodeContext(ea.now);
							ea.Use();
						}
						else if (ea.middleClick)
						{
							//SELECT ONLY
							ea.targetNode = n;
							ea.Use();
						}
						break;
					}
					// else if (ea.mouseReleased)
					// {
					// 	if (ea.targetNode) ea.targetNode.eData.isDraggable = false;

					// 	if (ea.leftClick && ea.isConnecting)
					// 	{
					// 		//END CONNECTION
					// 	}
					// 	break;
					// }
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
							ea.Use();
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
						ea.Use();
					}
					else if (ea.middleClick)
					{
						ea.isConnecting = false;
						ea.targetNode = null;
						ea.Use();
					}
				}
				else if (ea.mouseDragged)
				{
					if (ea.leftClick && ea.altKey || ea.middleClick)
					{
						//PAN VIEW
						PanView(ea.delta);
						gridDrag = ea.delta;
						ea.Use();
					}
				}
				// else if (ea.mouseReleased)
				// {
				// 	if (ea.isConnecting)
				// 	{
				// 		//ADD NODE CONTEXT
				// 		//(Only if implementing drag from node to create a connection)
				// 		// ShowAddNodeContext(e);
				// 	}
				// }
			}
			//----------- Keyboard Events -------------
			if (ea.keyDown)
			{
				switch (ea.keyCode)
				{
					case KeyCode.Escape:
						ea.isConnecting = false;
						ea.targetNode = null;
						ea.Use();
						break;
					case KeyCode.Delete:
						DeleteNode(ea.targetNode, confirmDelete);
						ea.Clear();
						ea.Use();
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
					//TODO Prevent currently selected BhaveAgent from losing focus

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
			m.AddItem(new GUIContent("Leafs/Suspender"), false, OnAddNodeContext, BHECommands.AddSuspender);

			m.ShowAsContext();
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
				case BHECommands.AddSuspender:
					newNode = CreateInstance<Suspender>();
					break;
				default: Debug.Log("Nothing created"); break;
			}

			//Handle if node is to be created
			if (newNode != null)
			{
				newNode.eData.rect = new Rect(ea.mousePosition.x - config.nodeSize.x * 0.5f, ea.mousePosition.y, config.nodeSize.x, config.nodeSize.y);
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

					//Ignore if it's the same
					if (activeAgent == selectedAgent)
					{
						Debug.Log("<Same Project>");
						return;
					}

					//Confirm load
					if (confirmLoad)
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

					//Colorize nodes according to play mode
					if (Application.isPlaying)
						SetAllNodeStates(NodeState.Failure);
					else
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

				if (activeTree == selectedBhaveTreeModule)
				{
					Debug.Log("<Same Project>");
					return;
				}

				if (projectIsDirty)
				{
					if (confirmLoad)
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

				//Colorize nodes according to play mode
				if (Application.isPlaying)
					SetAllNodeStates(NodeState.Failure);
				else
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
		static BhaveAgent activeAgent;
		static BhaveTree activeTree;   //The main bhave tree that this editor is currently working on
		static string moduleAssetPath = "Assets/";
		public static void SetActiveAgent(BhaveAgent agent) => activeAgent = agent;
		public static void SetActiveTree(BhaveTree tree) => activeTree = tree;
		public bool currentProjectIsAgent => activeAgent != null;

		//Flags
		static bool autosave;
		static bool projectIsDirty = false;     //Needs to be static for external autosave
		bool projectHasBeenSaved = false;
		bool confirmLoad = true;
		bool confirmDelete = false;

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
		public void Load(Object project)
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

					// if (showNofication)
					// 	ShowNotification(new GUIContent(string.Format("{0} saved ", activeAgent.name)));

					//Aren't agents already saved?

					//Reset flags
					// projectIsDirty = false;
					// projectHasBeenSaved = true;
				}
				//BhaveTree Module
				else if (activeTree)
				{
					if (showNofication)
						ShowNotification(new GUIContent(string.Format("{0} saved ", activeTree.name)));

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
				startNode.eData.rect.y + startNode.eData.rect.height - config.lineThickness * thicknessFineTune);          //Bottom

			//End node: Top Center
			var end = new Vector2(
				endNode.eData.rect.x + endNode.eData.rect.width * 0.5f,         //Center
				endNode.eData.rect.y + config.lineThickness * thicknessFineTune);                                    //Top

			//Set color
			Color? connColour;
			switch (endNode.state)
			{
				case NodeState.Pending:
					connColour = config.runningLineColor;
					break;
				case NodeState.Success:
					connColour = config.runningLineColor;
					break;
				default: connColour = config.defaultLineColour; break;
			}

			//Draw
			DrawConnection(start, end, connColour);
		}

		//Draw a connection using raw positions
		internal static void DrawConnection(Vector2 startPos, Vector2 endPos, Color? color = null)
		{
			var finalColor = color ?? config.defaultLineColour;

			switch (config.connectionStyle)
			{
				default:
				case BHEConnectionStyle.Square:
				case BHEConnectionStyle.Linear:
					Handles.DrawBezier(startPos, endPos, startPos, endPos, finalColor, null, config.lineThickness);
					break;
				case BHEConnectionStyle.Bezier:
					Vector2 startTan = startPos - Vector2.down * config.bezierTangent;
					Vector2 endTan = endPos - Vector2.up * config.bezierTangent;
					Handles.DrawBezier(startPos, endPos, startTan, endTan, finalColor, null, config.lineThickness);
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
}
#endif