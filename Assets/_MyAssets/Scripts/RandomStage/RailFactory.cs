using Cinemachine;
using PlayerMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomStage
{
    public class RailFactory : MonoBehaviour
    {
        private const float RayMaxDistance = 1000f;

        [SerializeField] private Rail m_railPrefab = default;
        [SerializeField] private LayerMask m_planetLayer = default;

        public Rail Create(Transform startPlanet, Transform nextPlanet)
        {
            // 2点を取得.
            var direction = (nextPlanet.position - startPlanet.position).normalized;
            var end = RayCastPointOnPlanet(startPlanet.position, direction);
            var start = RayCastPointOnPlanet(nextPlanet.position, -direction);

            // 生成.
            var rail = Instantiate(m_railPrefab, start, Quaternion.identity);
            
            // 設定に必要なコンポーネントを取得.
            var path = rail.GetComponent<CinemachineSmoothPath>();
            var targetGroup = rail.GetComponentInChildren<CinemachineTargetGroup>();

            // 目標の設定.
            rail.GoalPlanet = nextPlanet;

            // pathの設定.（直線）
            path.m_Waypoints = new CinemachineSmoothPath.Waypoint[]
            {
                new CinemachineSmoothPath.Waypoint(){ position = Vector3.zero}, // ローカル座標で設定.
                new CinemachineSmoothPath.Waypoint(){ position = end - start},
            };

            // カメラの設定.
            targetGroup.AddMember(startPlanet, 1f, 1f);
            targetGroup.AddMember(nextPlanet, 1f, 1f);

            return rail;
        }

        private Vector3 RayCastPointOnPlanet(Vector3 origin, Vector3 direction)
        {
            Physics.Raycast(origin, direction, out RaycastHit hit, RayMaxDistance, m_planetLayer.value);
            return hit.point;
        }
    }
}