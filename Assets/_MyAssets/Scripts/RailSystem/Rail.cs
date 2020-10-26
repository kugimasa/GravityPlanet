using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace RailSystem
{
    [RequireComponent(typeof(RailSetter))]
    public class Rail : MonoBehaviour, IRail
    {
        [Serializable]
        private class CameraActivation
        {
            public float m_rate = default;
            public GameObject m_activated = default;
            public CinemachineBlendDefinition.Style m_blendStyle = default;
        }

        [Header("Activation")]
        [SerializeField] private CameraActivation[] m_cameraActivations = default;

        [Header("Tracking")]
        [SerializeField] private Transform m_trackingObject = default;
        [SerializeField] private float m_trackingDelay = 0.5f;

        private RailSetter m_railSetter;
        private CinemachineBrain m_cameraBrain;

        public event EventHandler<Transform> OnMoveFinished;

        private void Start()
        {
            m_railSetter = GetComponent<RailSetter>();
            m_cameraBrain = Camera.main.GetComponent<CinemachineBrain>();
        }

        public void Move(Transform target)
        {
            StartCoroutine(MoveAlongPathCoroutine(target));
        }

        private IEnumerator MoveAlongPathCoroutine(Transform target)
        {
            var timer = 0f;
            var rate = 0f;
            var previousStyle = m_cameraBrain.m_DefaultBlend.m_Style;

            while ((rate = m_railSetter.GetRate(timer)) < 1f)
            {
                yield return null;

                // プレイヤーの移動処理.
                timer += Time.deltaTime;
                target.position = m_railSetter.GetRailPosition(rate);

                // 追従処理.
                if (m_trackingObject != null)
                {
                    m_trackingObject.position = m_railSetter.GetRailPosition(m_railSetter.GetRate(timer - m_trackingDelay));
                }

                // カメラ切り替え処理.
                SetCameraActives(rate);
            }

            // カメラを元に戻す.
            SetCameraOff(previousStyle);

            // イベント発火.
            OnMoveFinished?.Invoke(this, m_railSetter.GoalPlanet);
        }

        private void SetCameraActives(float rate)
        {
            bool foundActive = false;

            foreach (var activation in m_cameraActivations.Reverse())
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
        }

        private void SetCameraOff(CinemachineBlendDefinition.Style defaultStyle)
        {
            foreach (var activation in m_cameraActivations)
            {
                activation.m_activated.SetActive(false);
            }
            m_cameraBrain.m_DefaultBlend.m_Style = defaultStyle;
        }
    }
}