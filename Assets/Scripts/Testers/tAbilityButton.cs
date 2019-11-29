﻿using System.Collections;
using System.Collections.Generic;
using StormRend.Abilities;
using StormRend.Utility.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace StormRend.Test
{
    [RequireComponent(typeof(Button))]
    public class tAbilityButton : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] Ability ability = null;
        [SerializeField] AbilityEvent onHover = null;
        [SerializeField] AbilityEvent onClick = null;
        Button button;

        void Awake()
        {
            button = GetComponent<Button>();
			button.GetComponentInChildren<TextMeshProUGUI>().text = ability?.name;	//Lazy
			button.GetComponentInChildren<Image>().sprite = ability?.icon;			//Lazy
        }	

		//Register
        void OnEnable() => button.onClick.AddListener(OnClick);
		void OnDisable() => button.onClick.RemoveAllListeners();

        void OnClick() => onClick.Invoke(ability);
		public void OnPointerEnter(PointerEventData eventData) => onHover.Invoke(ability);
	}
}