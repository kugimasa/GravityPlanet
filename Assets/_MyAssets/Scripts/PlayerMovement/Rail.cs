using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerMovement
{
    [RequireComponent(typeof(CinemachineSmoothPath))]
    public class Rail : MonoBehaviour
    {
        public Transform GoalPlanet => m_goalPlanet;
        [SerializeField] private Transform m_goalPlanet = default;
        [SerializeField] private float m_pathDuration = 1f;
        [SerializeField] private GameObject m_trackingCamera = default;

        private CinemachineSmoothPath m_path;
        private Transform m_playerTransform;
        private float m_pathStartTime;

        public void MoveAlongPath(Transform targetTransform)
        {
            m_playerTransform = targetTransform;
            m_pathStartTime = Time.time;
            m_trackingCamera.SetActive(true);
        }

        private void Start()
        {
            m_path = GetComponent<CinemachineSmoothPath>();
        }

        private void Update()
        {
            if (m_playerTransform == null) return;

            //シネマシン移動処理
            var t = Time.time - m_pathStartTime;
            m_playerTransform.position = m_path.EvaluatePositionAtUnit(t / m_pathDuration, CinemachinePathBase.PositionUnits.Normalized);
            if (t > m_pathDuration)
            {
                m_playerTransform = null;
                m_trackingCamera.SetActive(false);
            }
        }
    }
}