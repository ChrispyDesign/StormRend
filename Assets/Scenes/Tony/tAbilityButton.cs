using System.Collections;
using System.Collections.Generic;
using StormRend.Abilities;
using StormRend.Utility.Events;
using UnityEngine;
using UnityEngine.UI;

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
        }
        void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            onClick.Invoke(ability);
        }

        void OnHover()
        {
            onHover.Invoke(ability);
        }
    }
}