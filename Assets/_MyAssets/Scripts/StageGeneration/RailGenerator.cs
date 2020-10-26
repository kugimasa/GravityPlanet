using UnityEngine;
using RailSystem;

namespace StageGeneration
{
    /// <summary>Railの設定を行うための抽象クラス</summary>
    /// <remarks>自動生成で、RandomStageGeneratorが使用することを想定</remarks>
    public abstract class RailGenerator : MonoBehaviour
    {
        private const float RayMaxDistance = 1000f;

        [Header("Planet")]
        [SerializeField] private LayerMask m_planetLayer = default;
        
        public IRail Rail { get; private set; }
        
        /// <summary>惑星fromからtoへのパスを設定. カメラ設定用にPlayerも必要</summary>
        public void Initialize(Transform from, Transform to, Transform player)
        {
            // コンポーネントを取得.
            Rail = this.GetComponent<IRail>();
            var railSetter = GetComponent<IRailSetter>();
            
            // パスの設定.
            var points = GetWayPoints(from, to);
            railSetter.SetWayPoints(points, to);
            
            // カメラの設定.
            SetCameraParameters(player);
        }

        /// <summary>惑星fromからtoへのパスを作成</summary>
        /// <remarks>グローバル座標を配列で返す</remarks>
        protected abstract Vector3[] GetWayPoints(Transform from, Transform to);

        /// <summary>カメラを設定</summary>
        protected abstract void SetCameraParameters(Transform player);

        protected Vector3 RayCastPointOnPlanet(Vector3 origin, Vector3 direction)
        {
            Physics.Raycast(origin, direction, out RaycastHit hit, RayMaxDistance, m_planetLayer.value);
            return hit.point;
        }
    }
}