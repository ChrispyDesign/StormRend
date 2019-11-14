using UnityEngine;
using StormRend.States.UI;
using StormRend.Units;
using StormRend.Systems.StateMachines;
using StormRend.Systems;

namespace StormRend.UI
{
	public class UnusedActionsChecker : MonoBehaviour
	{
		[SerializeField] string title = null;
		[SerializeField] string details = null;
		[SerializeField] OnState confirmationPanel;

		InfoPanel infoPanel;
		Animator animator;
		UnitRegistry unitRegistry;
		GameDirector gd;
		UltraStateMachine usm;

		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			animator = GetComponent<Animator>();

			Debug.Assert(animator, "There are no Animator in the scene. " + typeof(UnusedActionsChecker));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(UnusedActionsChecker));
		}
		private void Start()
		{
			gd = GameDirector.current;
			unitRegistry = UnitRegistry.current;
			usm = gd.gameObject.GetComponent<UltraStateMachine>();
		}

		public void OnEnter()
		{
			animator.SetInteger("Animation", 1);
			infoPanel.ShowPanel(title, 1, details);
		}

		public void OnExit()
		{
			animator.SetInteger("Animation", 2);
			infoPanel.UnShowPanel();
		}

		public void CheckMovesAvailable()
		{
			bool allUnitsHaveAttacked = true;

			foreach (AllyUnit unit in unitRegistry.GetUnitsByType<AllyUnit>())
			{
				if (unit.canAct) allUnitsHaveAttacked = false;
			}

			if (!allUnitsHaveAttacked)
				usm.Stack(confirmationPanel);
			else
				usm.NextTurn();
		}

		//dont need this just plug in the usm directly in the unity event
		// public void ClosePanel()
		// {
		// 	usm.UnStack();
		// }
	}
}