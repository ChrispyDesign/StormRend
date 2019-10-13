using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.Systems.Mapping
{
	/// <summary>
	/// Base tile class. Holds a list of connections to neighbouring tiles. 
	/// </summary>
	public abstract class Tile : MonoBehaviour //, IPointerEnterHandler//, IPointerExitHandler, IPointerClickHandler
	{
#if UNITY_EDITOR
		public Color editorColor = Color.white;
#endif
		public int ID;
		//public Map owner;		//TODO implement this!
		public HashSet<Tile> connections = new HashSet<Tile>();

		public float cost = 1;

		public float G = float.MaxValue;
		public float H = float.MaxValue;
		public float F = 0;

		public void Connect(Tile to) => connections.Add(to);
		public void Disconnect(Tile from) => connections.Remove(from);
		public void DisconnectAll() => connections.Clear();
	}
}
