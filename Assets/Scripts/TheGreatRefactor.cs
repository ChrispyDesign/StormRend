using UnityEngine;

namespace StormRend
{
    namespace The.Great.Refactor
    {
        public class Architecture

        public class StateMachineImplementation
        {
            public class State : ScriptableObject
            {
                public void OnEnter() { }
                public void OnUpdate() { }
                public void OnExit() { }
            }
            public class StackState : State
            {
                public void OnCover() { }
                public void OnUncover() { }
            }
            public class TurnState : State
            {
                public void OnNext() { }
            }


            public class CoreStateMachine
            {
                public virtual State currentState { get; private set; }

                //--------------------------------
                public void Switch(State state) 
                { 
                    currentState?.OnExit();
                    currentState = state;

                }
                protected virtual void Update()
                {
                    
                }
            }

            public class TurnBasedStackStateMachine : CoreStateMachine
            {
                protected Stack<State> states = new Stack<State>();

                public new State currentState
                {
                    get;
                }

                //-----------------------------------------------
                public void Push(StackState stackState) { }
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
                //-----------------
                public void Insert(State state) {}
                public void Remove(State state) {}
                public void NextTurn() { }
                public void PrevTurn() { }   //?
                //-----------------
            }
        }


        //Renames

        //Unit Testing

        //Conventions
        public class Conventions
        {
            //Fields/Symbols
            [SerializeField] float privateShownOnInspector;
            [HideInInspector] [SerializeField] float PrivateNotShownOnInspectorButSerialized;
            public float avoidMe;     //Free variable that can be modified by anything and anyone

            //Properties
            //Shown on inspector, but read only in the assembly/codebase
            [SerializeField] float _propertyBackingField;
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
    }
}