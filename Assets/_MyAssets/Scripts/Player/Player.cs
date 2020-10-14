using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Rails;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float m_gravityScale = 9.8f;
        [SerializeField] private float m_walkSpeed = 3f;
        [SerializeField] private LayerMask m_groundLayer = default;
        [SerializeField] private float m_distanceOnGround = 1.5f;
        [SerializeField] private float m_pathDuration = 1.0f;
        [SerializeField] private Transform m_currentPlanet = default;
        
        private Transform m_Transform = default;
        private Rigidbody m_Rigidbody = default;
        private Rail m_Rail = default;
        
        private float m_speedAlongGravity = 0f;

        private CinemachineSmoothPath m_nowCinemachine;
        private float m_pathStartTime = 0f;
        private void Start()
        {
            m_Transform = transform;
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Rail"))
            {
                var path = other.GetComponent<CinemachineSmoothPath>();
                m_Rail = other.GetComponent<Rail>();
                m_currentPlanet = m_Rail.GoalPlanet;
                m_nowCinemachine = path;
                m_pathStartTime = Time.time;
            }
        }

        private void Update()
        {
            if (m_nowCinemachine != null)
            {
                //シネマシン移動処理
                var t = Time.time - m_pathStartTime;
                m_Transform.position = m_nowCinemachine.EvaluatePositionAtUnit(t/m_pathDuration, CinemachinePathBase.PositionUnits.Normalized);
                if (t > m_pathDuration)
                {
                    m_nowCinemachine = null;
                }
            }
        }

        private void FixedUpdate()
        {
            // 重力.
            var gravityDirection = (m_currentPlanet.position - this.m_Transform.position).normalized; // 原点が重力源.
            // 地面に立っている時は重力方向の速さを0にする.
            if (Physics.Raycast(this.m_Transform.position, gravityDirection, this.m_distanceOnGround, this.m_groundLayer.value))
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
            var forward = Vector3.ProjectOnPlane(this.m_Transform.forward, upwards);
            this.m_Transform.rotation = Quaternion.LookRotation(forward, upwards);

            // 移動.
            var vInput = Input.GetAxis("Vertical");
            var hInput = Input.GetAxis("Horizontal");
            var walkDirection = (this.m_Transform.forward * vInput + this.m_Transform.right * hInput).normalized;
            var walkVelocity = walkDirection * this.m_walkSpeed;

            // Rigidbodyに反映. (速度制御)
            this.m_Rigidbody.velocity = gravityVelocity + walkVelocity;
        }

        private void OnDrawGizmos()
        {
            Vector3 start = this.transform.position;
            Vector3 end = start - this.transform.up * this.m_distanceOnGround;
            Gizmos.DrawLine(start, end);
        }
    }
}