using StormRend.AI;

namespace StormRend.States
{
    public class EnemyTurnState : TurnState
    {
        AIController aiCtrl;



        void Awake()
        {
            aiCtrl = FindObjectOfType<AIController>();
        }

        internal override void OnEnter()
        {
            base.OnEnter();

            //Run AI stuff
            aiCtrl.StartAITurn();
        }

        /* Brainstorm
        Enter
          - StartAITurn

        Update
          -
          
        Exit

        */

    }
}
