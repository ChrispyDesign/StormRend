using UnityEngine;

namespace StormRend.Defunct
{
    public class EnemyTurn : ShitState
    {
        [SerializeField] private TurnManager m_turnManager;

        // enemy turn timer
        [SerializeField] private float m_enemyTurnTime = 1f;
        private float m_timer;

        public EnemyTurn(TurnManager turnManager)
        {
            m_turnManager = turnManager;
        }

        public override void OnEnter()
        {
            m_timer = 0;
        }

        public override void OnUpdate(ShitStateMachine stateMachine)
        {
            //while (m_timer < m_enemyTurnTime)
            //{
            //    m_timer += Time.deltaTime;
            //    return;
            //}

            //m_turnManager.PlayerTurn();
        }

        public override void OnExit()
        {
            m_turnManager.ResetPlayerVariables();
        }
    }
}