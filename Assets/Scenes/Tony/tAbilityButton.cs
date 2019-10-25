using System.Collections;
using System.Collections.Generic;
using StormRend.Abilities;
using StormRend.Utility.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StormRend.Test
{
    [RequireComponent(typeof(Button))]
    public class tAbilityButton : MonoBehaviour
    {
        [SerializeField] Ability ability;
        [SerializeField] AbilityEvent onHover;
        [SerializeField] AbilityEvent onClick;
        Button button;

        void Awake()
        {
            button = GetComponent<Button>();
			button.GetComponentInChildren<TextMeshProUGUI>().text = ability?.name;
        }
        void OnEnable()
        {
            button.onClick.AddListener(OnClick);
        }
		void OnDisable()
		{
			button.onClick.RemoveAllListeners();
		}

        void OnClick()
        {
            onClick.Invoke(ability);
        }

        public void OnHover()
        {
            onHover.Invoke(ability);
        }
    }
}