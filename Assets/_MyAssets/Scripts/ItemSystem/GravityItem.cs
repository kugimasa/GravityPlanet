using GravitySystem;
using UnityEngine;

namespace ItemSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravityItem : Item
    {
        private Transform m_transform = default;
        private Rigidbody m_rigidbody = default;
        private ICalcVelocity m_calcVelocity = default;

        private void Start()
        {
            m_transform = transform;
            m_rigidbody = GetComponent<Rigidbody>();
            m_calcVelocity = GetComponent<ICalcVelocity>();
        }

        private void FixedUpdate()
        {
            m_rigidbody.velocity = m_calcVelocity.GetVelocity(out Vector3 direction);
            if(direction != Vector3.zero)
            {
                m_transform.up = -direction;
            }
        }
    }
}
