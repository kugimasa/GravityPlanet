using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ChasePlayerItem : MonoBehaviour
    {
        [SerializeField] private Transform m_chaseTargetTransform=default;
        [SerializeField] private float m_chaseSpeed=3.0f;
        [SerializeField] private float m_chaseLength = 5.0f;
        [SerializeField] private float m_chaseEndLength = 7.0f;
        [SerializeField] private string m_playerTag = "Player";

        private Transform m_transform = default;
        bool m_chaseNow = false;
        void Start()
        {
            m_transform = transform;
            m_chaseTargetTransform = GameObject.FindGameObjectWithTag(m_playerTag).transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_chaseTargetTransform == null) return;
            //追跡のonoff
            var distance = Vector3.Distance(m_chaseTargetTransform.position, m_transform.position);
            if (distance < m_chaseLength) m_chaseNow = true;
            else if (distance > m_chaseEndLength) m_chaseNow = false;

            //追跡処理
            if (m_chaseNow)
            {
                var direction = (m_chaseTargetTransform.position - m_transform.position).normalized;
                m_transform.position += direction * Time.deltaTime * m_chaseSpeed;
            }
        }
    }
}
