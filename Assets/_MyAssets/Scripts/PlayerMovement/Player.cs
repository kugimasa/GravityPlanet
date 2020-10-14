using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerMovement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float m_gravityScale = 9.8f;
        [SerializeField] private float m_walkSpeed = 3f;
        
        [Header("Ground")]
        [SerializeField] private LayerMask m_groundLayer = default;
        [SerializeField] private float m_distanceOnGround = 1.5f;

        [Header("PathMove")]
        [SerializeField] private string m_railTag = "Rail";
        [SerializeField] private Transform m_currentPlanet = default;
        
        private Transform m_transform = default;
        private Rigidbody m_rigidbody = default;
        
        private float m_speedAlongGravity = 0f;

        private void Start()
        {
            m_transform = transform;
            m_rigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(m_railTag))
            {
                var rail = other.GetComponent<Rail>();
                m_currentPlanet = rail.GoalPlanet;
                rail.MoveAlongPath(m_transform);
            }
        }

        private void FixedUpdate()
        {
            // 重力.
            var gravityDirection = (m_currentPlanet.position - this.m_transform.position).normalized; // 原点が重力源.
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

            // 回転.
            var upwards = -gravityDirection;
            var forward = Vector3.ProjectOnPlane(this.m_transform.forward, upwards);
            this.m_transform.rotation = Quaternion.LookRotation(forward, upwards);

            // 移動.
            var vInput = Input.GetAxis("Vertical");
            var hInput = Input.GetAxis("Horizontal");
            var walkDirection = (this.m_transform.forward * vInput + this.m_transform.right * hInput).normalized;
            var walkVelocity = walkDirection * this.m_walkSpeed;

            // Rigidbodyに反映. (速度制御)
            this.m_rigidbody.velocity = gravityVelocity + walkVelocity;
        }

        private void OnDrawGizmos()
        {
            Vector3 start = this.transform.position;
            Vector3 end = start - this.transform.up * this.m_distanceOnGround;
            Gizmos.DrawLine(start, end);
        }
    }
}