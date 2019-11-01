using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace StormRend.UI
{
	public class FinishTurn : MonoBehaviour
	{
		[SerializeField] string title;
		[SerializeField] string details;

		InfoPanel infoPanel;
		Animator animator;

		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			animator = FindObjectOfType<Animator>();

			Debug.Assert(animator, "There are no Animator in the scene. " + typeof(FinishTurn));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(FinishTurn));
		}

		public void OnEnter()
		{
			infoPanel.ShowPanel(title, details);
			animator.SetInteger("Animation", 1);
		}

		public void OnExit()
		{
			animator.SetInteger("Animation", 2);
			infoPanel.UnShowPanel();
		}
	}
}