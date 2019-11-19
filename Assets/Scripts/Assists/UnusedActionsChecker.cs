using UnityEngine;
using StormRend.UI;
using StormRend.States.UI;
using StormRend.Units;
using StormRend.Systems.StateMachines;
using StormRend.Systems;
using UnityEngine.EventSystems;

namespace StormRend.Assists
{
	public class UnusedActionsChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[SerializeField] string title = null;
		[SerializeField] string details = null;
		[SerializeField] OnState confirmationPanel;

		InfoPanel infoPanel = null;
		Animator anim = null;
		UnitRegistry ur = null;
		GameDirector gd = null;
		UltraStateMachine usm = null;

		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			anim = GetComponent<Animator>();

			Debug.Assert(anim, "There are no Animator in the scene. " + typeof(UnusedActionsChecker));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(UnusedActionsChecker));
		}
		private void Start()
		{
			gd = GameDirector.current;
			ur = UnitRegistry.current;
			usm = gd.GetComponent<UltraStateMachine>();
		}

		public void CheckMovesAvailable()
		{
			infoPanel.UnShowPanel(true);

			bool allUnitsHaveAttacked = true;

			foreach (AllyUnit unit in ur.GetAliveUnitsByType<AllyUnit>())
			{
				if (unit.canAct) allUnitsHaveAttacked = false;
			}

			if (!allUnitsHaveAttacked)
				usm.Stack(confirmationPanel);
			else
				usm.NextTurn();
		}

		//Event system callbacks
		public void OnPointerEnter(PointerEventData eventData)
		{
			anim.SetBool("OnHover", true);
			infoPanel.ShowPanel(title, 1, details);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			anim.SetBool("OnHover", false);
			infoPanel.UnShowPanel();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			anim.SetTrigger("OnClick");
			usm.Stack(confirmationPanel);
		}
	}
}