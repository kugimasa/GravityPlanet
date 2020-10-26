using GravitySystem;
using UnityEngine;

namespace PlayerMovement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour, IInputReciever
    {
        [SerializeField] private float m_walkSpeed = 3f;

        private ICalcVelocity m_gravity = default;
        private Transform m_transform = default;
        private Rigidbody m_rigidbody = default;

        public IPlayerInput PlayerInput { get; set; } = new PlayerInputAxis();

        private void Start()
        {
            m_transform = transform;
            m_rigidbody = GetComponent<Rigidbody>();
            m_gravity = GetComponent<ICalcVelocity>();
        }

        private void FixedUpdate()
        {
            // 重力.
            var gravityVelocity = m_gravity.GetVelocity(out Vector3 direction);

            // 回転.
            if (direction != Vector3.zero)
            {
                var upwards = -direction;
                var forward = Vector3.ProjectOnPlane(m_transform.forward, upwards);
                m_transform.rotation = Quaternion.LookRotation(forward, upwards);
            }

            // 移動.
            var move = PlayerInput.MoveVector();
            var walkDirection = m_transform.right * move.x + m_transform.forward * move.y;
            var walkVelocity = walkDirection * m_walkSpeed;

            // Rigidbodyに反映. (速度制御)
            m_rigidbody.velocity = gravityVelocity + walkVelocity;
        }
    }
}