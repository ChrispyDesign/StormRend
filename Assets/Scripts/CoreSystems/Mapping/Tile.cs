using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StormRend.Systems.Mapping
{
	public class Tile : MonoBehaviour, IPointerEnterHandler//, IPointerExitHandler, IPointerClickHandler
	{
        public int ID;




        List<Link> links = new List<Link>();
        float G = float.MaxValue;
        float H = float.MaxValue;
        float F = 0;

        public void Connect(Tile from, Tile to, float cost = 1)
        {
            links.Add(new Link(to, cost));
        }

		public void OnPointerEnter(PointerEventData eventData)
		{
			throw new System.NotImplementedException();
		}
	}
}
