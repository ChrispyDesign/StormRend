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
		
        public List<Link> links = new List<Link>();
        public float G = float.MaxValue;
        public float H = float.MaxValue;
        public float F = 0;

		//Connect to a tile
        public void Connect(Tile to, float cost = 1f) => links.Add(new Link(to, cost));

		//Disconnect from a tile
		public void Disconnect(Tile from) => links.RemoveAll(x => x.target == from);
	}
}
