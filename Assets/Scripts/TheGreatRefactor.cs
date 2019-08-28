using UnityEngine;

namespace StormRend
{
    namespace The.Great.Refactor
    {
        public class Architecture
        {
            /*
			- Better and improved turned based state machine

			public class TurnBasedStackableStateMachine
			+ Push(StackState)	//For pause etc
			+ Pop()

			+ Switch(StackState)
			+ Next()
			+ Previous()
            
			public class State
			+ OnCover()
			+ OnUncover()
			+ OnEnter()
			+ OnExit()
			+ OnUpdate()      */
            
            public class State : ScriptableObject
            {
                void OnEnter() {}
                void OnUpdate() {}
                void OnExit() {}
            }
            public class StackState : State
            {
                void OnCover() {}
                void OnUncover() {}
            }
            public class TurnState : State
            {
                public void OnNext() {}
            }


            public class StateMachine
            {
                public void Switch(State state) {}
                public State GetCurrentState()
                {
                    return new State();
                }
            }

            public class StackableStateMachine : StateMachine
            {
                public void Push(StackState stackState) {}
                public StackState Pop()
                {
                    return new StackState();
                }
                public StackState Peek()
                {
                    return new StackState();
                }
                public void Update()
                {

                }
            }

            public class TurnBasedStackableStateMachine : StackableStateMachine
            {
                public void NextTurn() {}
                public void PrevTurn() {}   //?
            }
        }

        public class Renames
        {
            


        }

        public class UnitTesting
        {

        }

        public class Conventions
        {
            //Fields/Symbols
            [SerializeField] float privateShownOnInspector;
            [HideInInspector] [SerializeField] float PrivateNotShownOnInspectorButSerialized;
            public float avoidMe;     //Free variable that can be modified by anything and anyone
            
            //Properties
            //Shown on inspector, but read only in the assembly/codebase
            [SerializeField] float _propertyBackingField;
            public float propertyBackingField {
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
        */}
    }
}