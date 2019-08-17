#if UNITY_EDITOR
namespace BhaVE.Editor.Nodes
{
	//The Editor component of a node
	interface IBHENode
	{
		void DrawContent();
		void DrawConnections();
	}
}
#endif