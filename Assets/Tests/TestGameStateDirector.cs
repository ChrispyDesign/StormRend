// using System.Collections;
// using System.Collections.Generic;
// using StormRend.Systems.StateMachines;
// using UnityEngine;

// namespace Tests
// {
//     public class TestGameStateDirector : TurnBasedStackStateMachine
//     {
//         [Space]
//         public StackState stack1;
//         public StackState stack2;

//         protected override void Update()
//         {
//             base.Update();

//             if (Input.GetKeyDown(KeyCode.Alpha1))
//                 Stack(stack1);
//             else if (Input.GetKeyDown(KeyCode.Alpha2))
//                 Stack(stack2);
//             else if (Input.GetKeyDown(KeyCode.Backspace))
//                 Debug.Log("Uncovered state: " + UnStack());
//             else if (Input.GetKeyDown(KeyCode.Return))
//                 NextTurn();
//         }
//     }
// }
