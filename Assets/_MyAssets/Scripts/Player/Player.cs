using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float gravityScale = 9.8f;
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private LayerMask groundLayer = default;
        [SerializeField] private float distanceOnGround = 1.5f;

        private Transform cashedTransform = default;
        private Rigidbody cashedRigidbody = default;

        private float speedAlongGravity = 0f;

        private CinemachineSmoothPath m_nowCinemachine;
        private float m_pathStartTime=0f;
        [SerializeField] float m_pathDuration = 1.0f;
        private void Start()
        {
            this.cashedTransform = this.transform;
            this.cashedRigidbody = this.GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Rail"))
            {
                var path = other.GetComponent<CinemachineSmoothPath>();
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
                cashedTransform.position=m_nowCinemachine.EvaluatePosition(t/m_pathDuration);
            }
        }

        private void FixedUpdate()
        {
            // 重力.
            var gravityDirection = (Vector3.zero - this.cashedTransform.position).normalized; // 原点が重力源.
            // 地面に立っている時は重力方向の速さを0にする.
            if (Physics.Raycast(this.cashedTransform.position, gravityDirection, this.distanceOnGround, this.groundLayer.value))
            {
                this.speedAlongGravity = 0f;
            }
            else
            {
                this.speedAlongGravity += this.gravityScale * Time.fixedDeltaTime;
            }
            var gravityVelocity = gravityDirection * this.speedAlongGravity;

            // 回転.
            var upwards = -gravityDirection;
            var forward = Vector3.ProjectOnPlane(this.cashedTransform.forward, upwards);
            this.cashedTransform.rotation = Quaternion.LookRotation(forward, upwards);

            // 移動.
            var vInput = Input.GetAxis("Vertical");
            var hInput = Input.GetAxis("Horizontal");
            var walkDirection = (this.cashedTransform.forward * vInput + this.cashedTransform.right * hInput).normalized;
            var walkVelocity = walkDirection * this.walkSpeed;

            // Rigidbodyに反映. (速度制御)
            this.cashedRigidbody.velocity = gravityVelocity + walkVelocity;
        }

        private void OnDrawGizmos()
        {
            Vector3 start = this.transform.position;
            Vector3 end = start - this.transform.up * this.distanceOnGround;
            Gizmos.DrawLine(start, end);
        }
    }
}