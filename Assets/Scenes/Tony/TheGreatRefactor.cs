using UnityEngine;

namespace The.Great.Refactor.Brainstorm
{
	/*

	Core
		Masters
		- GameDirector
		- UltraStateMachine
		- AudioSystem, AudioSource
		- EventSystem, StandaloneInputModule
		TurnStates
			AllyState
			- AllyTurnState
			- AudioSource, AudioSystem
			EnemyState
			- EnemyTurnState
			- AudioSource, AudioSystem
		Controllers
			UserInputHandler
			- UserInputHandler
			- UnitSelectAudioRelay
			- AudioSystem, AudioSource
			UnitRegistry

			BhaVE
		Data
			Blizzard
			Glory
			TileHighlights
		[UI]

	GameDirector:
	- Helps control the UltraStateMachine as well as other side functionality ie. handling pause, scene management
	-  

	------------------- Brainstorm
	- Glory and Blizzard are essentially just integer numbers
		Implementation Ideas:
		- Use advanced SO variables with inbuilt SO events that trigger when changed
		- global Scriptable object that holds all the crucial data
	- Selected Unit
		- SO variable
	- Current active units
		- Just find all units at start of scene

	------------------- Notification system
	What to notify about:
	- Invalid action/move
	- 

	------------------- Next Turn Sequence
	NextTurnButton.OnClick() => UltraStateMachine.NextTurn()

	USM:
	currentState.OnExit()
	UltraStateMachine.currentState = nextState;
	currentState.OnEnter()

	EnemyTurnState
	foreach e in enemies
		e.ai.Tick()
	TickCrystals()

	------------------ Execution Order of Critical Game Methods
	Critical methods:
	- AnimateUnit.CalculateMoveTiles()
	- UserInputHandler.OnStateChanged()
	- AutoUnitSelector.OnTurnEnter
	- GameDirector.SafeNextTurn()
	- GameDirector.CheckAndPerformGameEnd()
	- GameDirector.CheckAndPerformGameEnding()
	- USM.NextTurn()
	- USM.Stack()
	- USM.UnStack()
	- AllyTurnState

	AllyTurnState.OnTurnEnter(State)
	- AudioSource.PlayOneShot
	- UnitTurnStarter.RunStartTurn
	- UIH.OnStateChanged
	- AutoUnitSelector.Onturnenter

	//Blizzard
	1. BlizzardController.Tick(), blizzard.value++, blizzard.onChange!
	2. BlizzardMeter.OnChange(), internalValue: 5, SOV: 6

	AnimateUnit.CalculateMoveTiles()
	Needs to be executed:
	- At the start of a turn for that turn's unit type ie. allyTurnState will calculate all AllyUnit's possible moves
						OR
	- When the unit is first selected on that turn
	- When an new unit is spawned in or summoned
		- This prevents units from being able to walk onto these new units
	
	AnimateUnit.CalculateTargetTiles()
	Needs to be executed:
	- when ability selected > AbilityButton.OnClick()
	- when ability hovered > AbilityButton.OnHover


	----------------- Animation Event Handlers
	Naming convention: [UnitType]AnimEventHandlers

	BaseAnimationDelegate(s)
	+ SetAbility(Ability) : Hook up to AnimateUnit.OnActed(Ability)
	+ Execute() : AnimationEvent
	+ DeathDissolve() : AnimationEvent
	+ Kill() : AnimationEvent	//Actually finally 'kills' the unit and sets the unit inactive

	BerserkerAnimationDelegates
	> FuriousSwing:
	> Provoke:

	ValkyrieAnimEventHandlers
	+ PlayJumpPFX()
	+ PlayLandPFX()

	> LightFall:
	0. Valkyrie.animator.SetTrigger("LightFallJump")
	1. Play "LightFallJump" anim + Jumping Particles
	2. Teleport(NewTile) @ "LightFallJump" End
	3. Play "LightFallLand" anim
	4. Contact() @ Appropriate point in time + Landing Particles

	> PiercingLight:

	SageAnimationDelegates
	> SoulCommune:
	> SpiritCrystal:
	> SafePassage:

	------------- AI ---------------
	Delegates:
	- GetAllUnits
	- FilterUnitsInRange
	- CheckUnitsInRange
	- FilterProvokingUnits
	- SortByHealth
	- SortByPriority
	- MoveToUnit
	- CheckIfImmobilised
	- CheckIfBlinded
	- CheckIfUnitsAdjacent
	
	AttackPriority
	- Provoked
	- Distance
	- Low to highest health
	- Ally Type
	- 


	Basic AI algorithm
	1. Find opponent within move range + 1 
	(To account for the fact that they can still hit you if they're next to you)
		2. Check if any of the opponents are provoking
				YES > Set as ONLY target and return success
		3. Check if any of the opponents are immediately adjacent
				YES > Set as only target and return success
		4. Sort targets from lowest health to highest health
		5. Remove all targets except the first
		6. Move toward the closest adjacent tile of this target

	2. If not in range check for double range
			Move toward the closest

	3. If not in double range check for triple range 
			Move toward the closest

	End Player Turn when all unit's have moved
	- If all units of assigned type 
	*/

	#region Conventions
	internal class Conventions
	{
	    //Fields/Symbols
	    // [SerializeField] float privateShownOnInspector;
	    // [HideInInspector, SerializeField] float PrivateNotShownOnInspectorButSerialized;
	    // public float avoidMe = 0f;     //Free variable that can be modified by anything and anyone

	    //Properties
	    //Shown on inspector, but read only in the assembly/codebase
	    // [SerializeField] float _propertyBackingField = 0;
	    // public float propertyBackingField
	    // {
	    //     get => _propertyBackingField;

	    // }

	    // void Something()
	    // {
	    //     Debug.Log("somethign");
	    // }

	    // void UseExpressionBodyMethodsForCleanerCode() => Debug.Log("This is clean!");


	    //Privates
	    // bool isPrivate = true;      //Implicit private

	    /*
		Big classes
		- Classes over 200-300 lines of code should be split up using partial

		Try catch blocks
		try
		{
			if (blah)
			else if (something)
			else
				throw new InvalidOperationException("This is illegal!");
		}
		catch (Exception e)
		{
			Debug.LogWarning(e);
		}
	*/
	}
	#endregion
}