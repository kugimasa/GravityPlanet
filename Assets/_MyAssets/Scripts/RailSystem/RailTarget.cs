using UnityEngine;

namespace RailSystem
{
    public class RailTarget : MonoBehaviour
    {
        private Transform m_transform = default;

        private void Start()
        {
            m_transform = transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IRail rail)) return;
            rail.Move(m_transform);
        }
    }
}