using UnityEngine;

namespace StateMachine
{
    public class WaitTimeState : IState
    {
        private readonly float m_waitTime;
        private float m_timer = default;

        /// <summary>waitTimeだけ待機する状態</summary>
        /// <remarks>Time.deltaTimeで計算しているため、Updateで呼ばれることを想定</remarks>
        public WaitTimeState(float waitTime) => m_waitTime = waitTime;
        
        /// <summary>残り時間（単位:秒）</summary>
        public float RestTime => m_waitTime - m_timer;

        public void OnStart() => m_timer = 0f;

        public bool OnNext()
        {
            m_timer += Time.deltaTime;
            return m_timer < m_waitTime;
        }
    }
}
