using System.Linq;
using pokoro.BhaVE.Core.Variables;
using StormRend.Abilities;
using StormRend.Enums;
using StormRend.Systems;
using StormRend.Units;
using StormRend.Utility.Attributes;
using StormRend.Variables;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StormRend.UI
{
    [RequireComponent(typeof(Button))]
    public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        //Inspector
        [SerializeField] Image buttonIcon = null;
        [SerializeField] GameObject buttonAssembly = null;

        [Header("SOVs")]
        [SerializeField] BhaveInt glory = null;
        [SerializeField] UnitVar selectedUnit = null;

        [Header("Ability Type Settings")]
        [SerializeField] AbilityType abilityType = AbilityType.Primary;
        [SerializeField] AbilityLevel abilityLevel = AbilityLevel.One;

        //Members
        Button button = null;
        Ability ability = null;
        UserInputHandler inputHandler = null;

        //Temp
        InfoPanel tempInfoPanel = null;

        //Inits
        void Awake()
        {
            inputHandler = FindObjectOfType<UserInputHandler>();
            tempInfoPanel = FindObjectOfType<InfoPanel>();
            button = GetComponent<Button>();

            //Asserts
            if (!inputHandler) {
                Debug.LogWarningFormat("No user input handler found. Disabling {0}", this.name);
                this.enabled = false;
            }

            Debug.Assert(glory, "No Glory SOV found!");
            Debug.Assert(selectedUnit, "No Unit SOV found!");

            Debug.Assert(buttonAssembly, "Button assembly reference required!");
            Debug.Assert(buttonIcon, "Button icon reference required!");
        }

        void Start()
        {
            UpdateAbility();
        }
        
        void OnEnable()
        {
            button.onClick.AddListener(OnClick);
            selectedUnit.onChanged += UpdateAbility;
        }
        void OnDisable()
        {
            button.onClick.RemoveAllListeners();
            selectedUnit.onChanged -= UpdateAbility;
        }

        void UpdateAbility()
        {
            var au = selectedUnit.value as AnimateUnit;

            //Unselected
            if (!au)
            {
                buttonAssembly?.SetActive(false);
            }
            //Selected
            else
            {
                buttonAssembly?.SetActive(true);

                //Update the ability from the selected ability type and level for this button
                ability = (from a in au.abilities
                           where a.type == abilityType
                           where a.level == (int)abilityLevel
                           select a).First();

                //Update icon
                if (buttonIcon) buttonIcon.sprite = ability.icon;
            }
        }

        //Core
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!ability) return;

            inputHandler.OnHoverPreview(ability);

            string[] details = new string[ability.level];
            for (int i = 0; i < ability.level; i++)
                details[i] = ability.descriptions[i];

            tempInfoPanel?.ShowPanel(ability.title, ability.level, details);
        }

        public void OnClick()
        {
            if (!ability) return;

            inputHandler.SelectAbility(ability);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inputHandler.OnUnhoverPreview();

            //Temp
            tempInfoPanel?.UnShowPanel();
        }
    }
}