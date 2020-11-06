using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravityItem : MonoBehaviour,INeedPlanet
    {
        [Header("Movement")]
        [SerializeField] private float m_gravityScale = 9.8f;

        [Header("Ground")]
        [SerializeField] private LayerMask m_groundLayer = default;
        [SerializeField] private string m_groundTag = "Ground";
        [SerializeField] private float m_distanceOnGround = 1.5f;

        [SerializeField] private Transform m_currentPlanet = default;
        private Transform m_transform = default;
        private Rigidbody m_rigidbody = default;

        private float m_speedAlongGravity = 0f;

        private void Start()
        {
            m_transform = transform;
            m_rigidbody = GetComponent<Rigidbody>();
        }

        //playerのコードをほぼ移植した
        //重いかもしれない
        private void FixedUpdate()
        {
            if (m_currentPlanet == null) return;
            // 重力.
            var gravityDirection = (m_currentPlanet.position - this.m_transform.position).normalized;
            // 地面に立っている時は重力方向の速さを0にする.
            if (Physics.Raycast(this.m_transform.position, gravityDirection, this.m_distanceOnGround, this.m_groundLayer.value))
            {
                this.m_speedAlongGravity = 0f;
            }
            else
            {
                this.m_speedAlongGravity += this.m_gravityScale * Time.fixedDeltaTime;
            }
            var gravityVelocity = gravityDirection * this.m_speedAlongGravity;
            // Rigidbodyに反映. (速度制御)
            this.m_rigidbody.velocity = gravityVelocity;
        }

        public void SupplyPlanet(Transform planetTransform)
        {
            m_currentPlanet = planetTransform;
            transform.up = (transform.position - planetTransform.position).normalized;
        }

    }
}
