using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace StormRend.UI
{
	public class FinishTurn : MonoBehaviour
	{
		[SerializeField] string title = null;
		[SerializeField] string details = null;

		InfoPanel infoPanel;
		Animator animator;

		void Awake()
		{
			infoPanel = FindObjectOfType<InfoPanel>();
			animator = GetComponent<Animator>();

			Debug.Assert(animator, "There are no Animator in the scene. " + typeof(FinishTurn));
			Debug.Assert(infoPanel, "There are no Info Panel Script in the scene. " + typeof(FinishTurn));
		}

		public void OnEnter()
		{
			animator.SetInteger("Animation", 1);
			infoPanel.ShowPanel(title, details, 1);
		}

		public void OnExit()
		{
			animator.SetInteger("Animation", 2);
			infoPanel.UnShowPanel();
		}
	}
}