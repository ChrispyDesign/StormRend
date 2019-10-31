using UnityEngine;

namespace The.Great.Refactor.Brainstorm
{
	/*
	------------------- Brainstorm
	- Glory and Blizzard are essentially just integer numbers
		Implementation Ideas:
		- Use advanced SO variables with inbuilt SO events that trigger when changed
		- global Scriptable object that holds all the crucial data
	- Selected Unit
		- SO variable
	- Current active units
		- Just find all units at start of scene

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

	

	*/

	#region Conventions
	internal class Conventions
	{
	    //Fields/Symbols
	    [SerializeField] float privateShownOnInspector;
	    [HideInInspector, SerializeField] float PrivateNotShownOnInspectorButSerialized;
	    public float avoidMe = 0f;     //Free variable that can be modified by anything and anyone

	    //Properties
	    //Shown on inspector, but read only in the assembly/codebase
	    [SerializeField] float _propertyBackingField = 0;
	    public float propertyBackingField
	    {
	        get => _propertyBackingField;

	    }

	    void Something()
	    {
	        Debug.Log("somethign");
	    }

	    void UseExpressionBodyMethodsForCleanerCode() => Debug.Log("This is clean!");


	    //Privates
	    bool isPrivate = true;      //Implicit private

	    /*
		Big classes
		- Classes over 200-300 lines of code should be split up using partial

		Try catch blocks
		try
		{
			if (blah)
			else if (bleh)
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