using UnityEngine;

namespace StateMachine
{
    public class PlayState : IState
    {
        private readonly Transform m_player = default;
        private readonly Transform m_goal = default;
        private readonly float m_goalThreshold = 1f;

        public float PlayTime { get; private set; }

        /// <summary>playerとgoalの距離がgoalThereshold以内になるまでの時間を測定する状態</summary>
        /// <remarks>Time.deltaTimeで計算しているため、Updateで呼ばれることを想定</remarks>
        public PlayState(Transform player, Transform goal, float goalThreshold)
        {
            m_player = player;
            m_goal = goal;
            m_goalThreshold = goalThreshold;
        }

        public void OnStart() => PlayTime = 0f;

        public bool OnNext()
        {
            PlayTime += Time.deltaTime;
            return Vector3.Distance(m_player.position, m_goal.position) > m_goalThreshold;
        }
    }
}
