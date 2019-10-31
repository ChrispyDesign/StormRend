using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using pokoro.BhaVE.Core.Events;
using StormRend.Variables;

namespace StormRend.UI
{
	public class AbilitySelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField] string details;

		public UnitVar selectedUnit;

		InfoPanel infoPanel;

		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(FinishTurn));
		}

		private void Update()
		{
			
		}

		void OnEnable()
		{
			//selectedUnit.onChanged += SelectedUnitOnChange;
		}
		void OnDisable()
		{
			//selectedUnit.onChanged -= SelectedUnitOnChange;
		}

		void SelectedUnitOnChange()
		{
			//selectedUnit.value.GetAbilities()
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			infoPanel.ShowPanel(details);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			infoPanel.UnShowPanel();
		}
	}
}