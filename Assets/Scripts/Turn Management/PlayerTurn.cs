using UnityEngine;

namespace StormRend.Defunct
{
    public class PlayerTurn : ShitState
    {
        [SerializeField] TurnManager m_turnManager;

        // turn timing variables
        private float m_turnTimer = 0;
        private float m_longestTurn = 0;
        private float m_totalTurnTime = 0;

        public PlayerTurn(TurnManager turnManager)
        {
            m_turnManager = turnManager;
        }

        public override void OnEnter()
        {
            m_turnTimer = 0;
        }

        public override void OnUpdate(ShitStateMachine stateMachine)
        {
            // increment turn timer
            m_turnTimer += Time.deltaTime;
        }

        public override void OnExit()
        {
            // update longest turn
            if (m_turnTimer > m_longestTurn)
                m_longestTurn = m_turnTimer;

            // update total turn time
            m_totalTurnTime += m_turnTimer;

            m_turnManager.ResetEnemyVariables();
        }
    }
}