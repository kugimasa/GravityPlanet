using UnityEngine;
using Cinemachine;

namespace StageGeneration
{
    /// <summary>直線でパスを作成し、カメラはプレイヤーを設定するRailGenerator</summary>
    public class RailGeneratorSimple : RailGenerator
    {
        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera[] m_followPlayerCameras = default;
        [SerializeField] private CinemachineVirtualCamera[] m_lookAtPlayerCameras = default;
        
        /// <summary>惑星fromからtoへのパスを直線で設定</summary>
        protected override Vector3[] GetWayPoints(Transform from, Transform to)
        {
            var direction = (to.position - from.position).normalized;
            var start = RayCastPointOnPlanet(to.position, -direction);
            var end = RayCastPointOnPlanet(from.position, direction);
            return new Vector3[] { start, end };
        }

        /// <summary>カメラの設定を行う. LookAtとFollowTargetを設定</summary>
        protected override void SetCameraParameters(Transform player)
        {
            foreach (var follow in m_followPlayerCameras) follow.Follow = player;
            foreach (var lookAt in m_lookAtPlayerCameras) lookAt.LookAt = player;
        }
    }
}