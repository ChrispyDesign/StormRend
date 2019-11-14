using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using StormRend.States.UI;
using StormRend.Units;
using StormRend.Systems.StateMachines;
using StormRend.Systems;

namespace StormRend.UI
{
	public class FinishTurn : MonoBehaviour
	{
		[SerializeField] string title = null;
		[SerializeField] string details = null;
		[SerializeField] OnState comfirmPanel;

		InfoPanel infoPanel;
		Animator animator;
		UnitRegistry unitRegistry;
		GameDirector gd;
		UltraStateMachine usm;

		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			animator = GetComponent<Animator>();

			Debug.Assert(animator, "There are no Animator in the scene. " + typeof(FinishTurn));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(FinishTurn));
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
			bool haveAllUnitsAttacked = false;

			foreach (AllyUnit unit in unitRegistry.GetUnitsByType<AllyUnit>())
			{
				if (unit.canAct)
					haveAllUnitsAttacked = true;
			}

			if (haveAllUnitsAttacked)
				usm.Stack(comfirmPanel);
			else
				usm.NextTurn();
		}

		public void ClosePanel()
		{
			usm.UnStack();
		}
	}
}