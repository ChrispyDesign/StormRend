using UnityEngine;
using StormRend.UI;
using StormRend.Units;
using StormRend.Systems.StateMachines;
using StormRend.Systems;
using UnityEngine.EventSystems;

namespace StormRend.Assists
{
	public class EndTurnButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[SerializeField] string title = "End Turn";
		[SerializeField] string details = "Ends your current turn";
		[SerializeField] State confirmationPanel;

		InfoPanel infoPanel = null;
		Animator anim = null;
		UnitRegistry ur = null;
		GameDirector gd = null;
		UltraStateMachine usm = null;

		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			anim = GetComponent<Animator>();

			Debug.Assert(anim, string.Format("[{0}] {1} not found!", this.name, typeof(Animator).Name));
			Debug.Assert(infoPanel, string.Format("[{0}] {1} not found!", this.name, typeof(InfoPanel).Name));
		}
		private void Start()
		{
			gd = GameDirector.current;
			ur = UnitRegistry.current;
			usm = gd.GetComponent<UltraStateMachine>();
		}


		//Event system callbacks
		public void OnPointerEnter(PointerEventData eventData)
		{
			anim.SetBool("OnHover", true);
			infoPanel?.ShowPanel(title, 1, details);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			anim.SetBool("OnHover", false);
			infoPanel?.UnShowPanel();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			anim.SetTrigger("OnClick");
			usm.Stack(confirmationPanel);
		}
	}
}