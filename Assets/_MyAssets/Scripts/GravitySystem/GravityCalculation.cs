using UnityEngine;

namespace GravitySystem
{
    public class GravityCalculation : MonoBehaviour, ICalcVelocity, IPlanetSetter
    {
        [SerializeField] private Transform m_currentPlanet = default;
        [SerializeField] private float m_gravityScale = 9.8f;
        [SerializeField] private LayerMask m_groundLayer = default;
        [SerializeField] private float m_distanceOnGround = 1.5f;

        private Transform m_transform = default;
        private float m_speedAlongGravity = 0f;

        public void SetPlanet(Transform planet) => m_currentPlanet = planet;

        /// <param name="gravityDirection">地面に接している場合は速さ0になるので、方向はoutで対応</param>
        /// <remarks>惑星が設定されていない場合はVector3.zeroになる</remarks>
        public Vector3 GetVelocity(out Vector3 direction)
        {
            if (m_currentPlanet == null)
            {
                direction = Vector3.zero;
                return Vector3.zero;
            }
            m_transform = m_transform ?? transform;

            // 重力.
            direction = (m_currentPlanet.position - m_transform.position).normalized;

            // 地面に立っている時は重力方向の速さを0にする.
            if (Physics.Raycast(this.m_transform.position, direction, this.m_distanceOnGround, this.m_groundLayer.value))
            {
                this.m_speedAlongGravity = 0f;
            }
            else
            {
                this.m_speedAlongGravity += this.m_gravityScale * Time.fixedDeltaTime;
            }
            var gravityVelocity = direction * this.m_speedAlongGravity;

            return gravityVelocity;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 start = transform.position;
            Vector3 end = start - transform.up * m_distanceOnGround;
            Gizmos.DrawLine(start, end);
        }
#endif
    }
}
