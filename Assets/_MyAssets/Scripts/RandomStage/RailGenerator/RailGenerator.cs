using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RandomStage
{
    [RequireComponent(typeof(Rail))]
    /// <summary>Railの設定を行うための抽象クラス</summary>
    /// <remarks>自動生成で、RandomStageGeneratorが使用することを想定</remarks>
    public abstract class RailGenerator : MonoBehaviour
    {
        private const float RayMaxDistance = 1000f;

        [Header("Planet")]
        [SerializeField] private LayerMask m_planetLayer = default;

        [Header("Path")]
        [SerializeField] private float m_moveSpeed = 10f;
        [SerializeField] private float m_minimumDuration = 1f;

        public Rail Rail { get; private set; }
        protected CinemachineSmoothPath m_path = default;

        /// <summary>惑星fromからtoへのパスを設定. カメラ設定用にPlayerも必要</summary>
        public void Initialize(Transform from, Transform to, Transform player)
        {
            // コンポーネントを取得.
            Rail = this.GetComponent<Rail>();
            m_path = Rail.GetComponent<CinemachineSmoothPath>();

            // パスの設定.
            SetWayPoints(from, to, out Vector3 start);
            transform.SetPositionAndRotation(start, Quaternion.identity);
            m_path.InvalidateDistanceCache();
            Rail.PathDuration = Mathf.Max(m_path.PathLength / m_moveSpeed, m_minimumDuration);

            // 目標の設定.
            Rail.GoalPlanet = to;

            // カメラの設定.
            SetCameraParameters(player);
        }

        /// <summary>惑星fromからtoへのパスを設定</summary>
        /// <remarks>スタート地点がローカル座標の原点となるように実装, スタート地点のグローバル座標をstartで返す</remarks>
        protected abstract void SetWayPoints(Transform from, Transform to, out Vector3 start);

        /// <summary>カメラを設定</summary>
        protected abstract void SetCameraParameters(Transform player);

        protected Vector3 RayCastPointOnPlanet(Vector3 origin, Vector3 direction)
        {
            Physics.Raycast(origin, direction, out RaycastHit hit, RayMaxDistance, m_planetLayer.value);
            return hit.point;
        }
    }
}