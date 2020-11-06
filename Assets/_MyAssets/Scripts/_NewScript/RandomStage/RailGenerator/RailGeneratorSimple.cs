using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RandomStage
{
    /// <summary>直線でパスを作成し、カメラはプレイヤーを設定するRailGenerator</summary>
    public class RailGeneratorSimple : RailGenerator
    {
        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera[] m_followPlayerCameras = default;
        [SerializeField] private CinemachineVirtualCamera[] m_lookAtPlayerCameras = default;

        /// <summary>惑星fromからtoへのパスを直線で設定</summary>
        protected override void SetWayPoints(Transform from, Transform to, out Vector3 start)
        {
            // 2点を取得.
            var direction = (to.position - from.position).normalized;
            var end = RayCastPointOnPlanet(from.position, direction);
            start = RayCastPointOnPlanet(to.position, -direction);

            // pathの設定.（直線）
            m_path.m_Waypoints = new CinemachineSmoothPath.Waypoint[]
            {
                new CinemachineSmoothPath.Waypoint(){ position = Vector3.zero}, // ローカル座標で設定.
                new CinemachineSmoothPath.Waypoint(){ position = end - start},
            };
        }

        /// <summary>カメラの設定を行う. LookAtとFollowTargetを設定</summary>
        protected override void SetCameraParameters(Transform player)
        {
            foreach (var follow in m_followPlayerCameras) follow.Follow = player;
            foreach (var lookAt in m_lookAtPlayerCameras) lookAt.LookAt = player;
        }
    }
}