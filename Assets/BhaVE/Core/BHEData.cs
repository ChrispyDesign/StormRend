#if UNITY_EDITOR
using UnityEngine;
using BhaVE.Core;

namespace BhaVE.Editor.Data
{
	[System.Serializable]
	public class BHEData
	{
		public BhaveTree owner;
		[TextArea]
		public string description;
		internal bool selected;
		internal bool isDraggable;
		public Rect rect;

		[Header("Advanced")]
		public int cardinality;
		public int breakpoint;

		public void SetOwner(BhaveTree owner) => this.owner = owner;

	}
}
#endif