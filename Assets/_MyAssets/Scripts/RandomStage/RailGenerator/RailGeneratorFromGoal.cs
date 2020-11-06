using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomStage
{
    /// <summary>直線でパスを作成し、カメラは着地点からの視点で設定するRailGenerator</summary>
    public class RailGeneratorFromGoal : RailGenerator
    {
        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera[] m_lookAtPlayerCameras = default;

        [Header("CirclePoints")]
        [SerializeField] private CirclePointPosition m_startPoint = default;
        [SerializeField] private CirclePointPosition m_endPoint = default;
        [SerializeField] private float m_pointOffset = 5f;

        protected override void SetCameraParameters(Transform player)
        {
            foreach (var lookAt in m_lookAtPlayerCameras) lookAt.LookAt = player;
        }

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

            // CirclePointの設定.
            var pathDir = (end - start).normalized;
            var cameraTransform = Camera.main.transform;

            m_startPoint.CircleCenter = start + pathDir * m_pointOffset;
            m_startPoint.CircleNormal = pathDir;
            m_startPoint.Target = cameraTransform;
            
            m_endPoint.CircleCenter = end - pathDir * m_pointOffset;
            m_endPoint.CircleNormal = -pathDir;
            m_endPoint.Target = cameraTransform;
        }
    }
}