using System;
using System.Linq;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;

namespace RandomStage
{
    [RequireComponent(typeof(CinemachineSmoothPath))]
    public class Rail : MonoBehaviour
    {
        [Serializable]
        private class RailActivation
        {
            public float m_rate = default;
            public GameObject m_activated = default;
            public CinemachineBlendDefinition.Style m_blendStyle = default;
        }

        public Transform GoalPlanet
        {
            get => m_goalPlanet;
            set => m_goalPlanet = value;
        }

        public float PathDuration
        {
            get => m_pathDuration;
            set => m_pathDuration = value;
        }

        [Header("Path")]
        [SerializeField] private Transform m_goalPlanet = default;
        [SerializeField] private float m_pathDuration = 1f;

        [Header("Activation")]
        [SerializeField] private RailActivation[] m_railActivations = default;

        [Header("Tracking")]
        [SerializeField] private Transform m_trackingObject = default;
        [SerializeField] private float m_trackingDelay = 0.1f;

        private CinemachineSmoothPath m_path;
        private CinemachineBrain m_cameraBrain;
        private CinemachineBlendDefinition.Style m_previousStyle;
        private Transform m_playerTransform;
        private float m_pathStartTime;

        public void MoveAlongPath(Transform targetTransform)
        {
            m_playerTransform = targetTransform;
            m_pathStartTime = Time.time;
            m_previousStyle = m_cameraBrain.m_DefaultBlend.m_Style;

            foreach(var activation in m_railActivations)
            {
                activation.m_activated.SetActive(activation.m_rate <= 0f);
            }
        }

        private void Start()
        {
            m_path = GetComponent<CinemachineSmoothPath>();
            m_cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
        }

        private void Update()
        {
            if (m_playerTransform == null) return;

            // プレイヤーの移動処理.
            var t = Time.time - m_pathStartTime;
            var rate = t / m_pathDuration;
            m_playerTransform.position = m_path.EvaluatePositionAtUnit(rate, CinemachinePathBase.PositionUnits.Normalized);

            // 追従処理.
            if(m_trackingObject != null)
            {
                m_trackingObject.position = m_path.EvaluatePositionAtUnit(rate - m_trackingDelay, CinemachinePathBase.PositionUnits.Normalized);
            }

            // カメラ切り替え処理.
            bool foundActive = false;
            foreach (var activation in m_railActivations.Reverse()) 
            {
                if (foundActive == false && activation.m_rate <= rate) 
                {
                    activation.m_activated.SetActive(true);
                    m_cameraBrain.m_DefaultBlend.m_Style = activation.m_blendStyle;
                    foundActive = true;
                }
                else
                {
                    activation.m_activated.SetActive(false);
                }
            }

            // 終了処理.    
            if (t > m_pathDuration)
            {
                m_playerTransform = null;
                foreach (var activation in m_railActivations)
                {
                    activation.m_activated.SetActive(false);
                }
                m_cameraBrain.m_DefaultBlend.m_Style = m_previousStyle;
            }
        }
    }
}