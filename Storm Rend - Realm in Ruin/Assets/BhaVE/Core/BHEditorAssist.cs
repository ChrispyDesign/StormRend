using BhaVE.Nodes;
using UnityEngine;

/// <summary>
/// BhaVE Editor assist tool for handling input events and node editing.
/// Holds editor states, current node selected etc.
/// </summary>
public struct BHEditorAssist
{
	///States
	public bool isOverNode, isConnecting;

	///Nodes
	public Node targetNode;
	public Node[] targetNodes;  //Maybe for multiselection?

	///Connections
	public Node connStartNode;      //Might not need these as subjectNode is initially the starting node and upon clicking on the new node, will become end node
	public Vector2 connStartPos;    //Might node need endpos

	///Events
	public Event now;

	/// <summary>
	/// Caches the current mouse position and delta
	/// <param name="current">Event.current</param>
	/// </summary>
	public void SetEvent(Event current) => now = current;

	//Mouse
	public Vector2 mousePosition => now.mousePosition;
	public Vector2 delta => now.delta;
	public bool leftClick => now.button == 0;
	public bool rightClick => now.button == 1;
	public bool middleClick => now.button == 2;
	public bool doubleClick => now.clickCount == 2;
	public bool mouseClicked => now.type == EventType.MouseDown;
	public bool mouseReleased => now.type == EventType.MouseUp;
	public bool mouseDragged => now.type == EventType.MouseDrag;

	//Keyboard
	public bool commandKey => now.modifiers == EventModifiers.Control || now.modifiers == EventModifiers.Command;
	public bool altKey => now.modifiers == EventModifiers.Alt;
	public bool keyDown => now.type == EventType.KeyDown;
	public bool keyUp => now.type == EventType.KeyUp;
	public KeyCode keyCode => now.keyCode;

	public void Clear()
	{
		isOverNode = isConnecting = false;
		connStartPos = new Vector2();
		targetNode = connStartNode = null;
		targetNodes = new Node[0];
	}
}