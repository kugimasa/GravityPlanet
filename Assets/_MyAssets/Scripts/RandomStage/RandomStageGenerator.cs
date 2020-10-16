using Cinemachine;
using PlayerMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomStage
{
    public class RandomStageGenerator : MonoBehaviour
    {
        private const float RayMaxDistance = 1000f;

        [Header("Planets")]
        [SerializeField] private Transform m_startPlanet = default;
        [SerializeField] private Transform m_goalPlanet = default;
        [SerializeField] private LayerMask m_planetLayer = default;

        [Header("Creation")]
        [SerializeField] private int m_planetCounts = 10;
        [SerializeField] private Vector2 m_noiseScaleParallel = new Vector2(-10f, 10f);
        [SerializeField] private Vector2 m_noiseScalePerpendicular = new Vector2(0f, 10f);
        
        [Header("Prefabs")]
        [SerializeField] private GameObject m_planetPrefab = default;
        [SerializeField] private Rail m_railPrefab = default;

        private void Start()
        {
            // ベースは等間隔で直線的.
            var _transform = transform;
            var startPos = m_startPlanet.position;
            var dr = (m_goalPlanet.position - startPos) / (m_planetCounts + 1);
            var direction = dr.normalized;
            List<Transform> planets = new List<Transform>(); // Rail作成用にPlanetをリストで保持しておく.
            planets.Add(m_startPlanet);
            for (int i = 0; i < m_planetCounts; i++)
            {
                // 基本位置
                var pos = startPos + (i + 1) * dr;
                
                // 乱数で基本位置からずらす.
                pos += Random.Range(m_noiseScaleParallel.x, m_noiseScaleParallel.y) * direction;
                var r = Random.Range(m_noiseScalePerpendicular.x, m_noiseScalePerpendicular.y);
                var theta = Random.Range(0f, 2*Mathf.PI);
                pos += Quaternion.LookRotation(direction) * new Vector3(r*Mathf.Cos(theta), r*Mathf.Sin(theta), 0f);
                
                // 生成.
                var instance = Instantiate(m_planetPrefab, pos, Quaternion.identity, _transform);
                planets.Add(instance.transform);
            }
            planets.Add(m_goalPlanet);

            // Planet間を直線で結んでRailを生成.
            Transform prePlanet = null;
            foreach(var nextPlanet in planets)
            {
                if(prePlanet != null)
                {
                    // 2点を取得.
                    var preToNextDirection = (nextPlanet.position - prePlanet.position).normalized;
                    var end = RayCastPointOnPlanet(prePlanet.position, preToNextDirection);
                    var start = RayCastPointOnPlanet(nextPlanet.position, -preToNextDirection);

                    #region Railの生成処理.
                    // わかりにくいので、Factoryクラスを後で作成する予定.
                    var rail = Instantiate(m_railPrefab, start, Quaternion.identity);

                    // 設定に必要なコンポーネントを取得.
                    var path = rail.GetComponent<CinemachineSmoothPath>();
                    var targetGroup = rail.GetComponentInChildren<CinemachineTargetGroup>();

                    // 目標の設定.
                    rail.GoalPlanet = nextPlanet;

                    // pathの設定.
                    path.m_Waypoints = new CinemachineSmoothPath.Waypoint[]
                    {
                        new CinemachineSmoothPath.Waypoint(){ position = Vector3.zero}, // ローカル座標で設定.
                        new CinemachineSmoothPath.Waypoint(){ position = end - start},
                    };

                    // カメラの設定.
                    targetGroup.AddMember(prePlanet, 1f, 1f);
                    targetGroup.AddMember(nextPlanet, 1f, 1f);
                    #endregion
                }

                prePlanet = nextPlanet;
            }
        }

        private Vector3 RayCastPointOnPlanet(Vector3 origin, Vector3 direction)
        {
            Physics.Raycast(origin, direction, out RaycastHit hit, RayMaxDistance, m_planetLayer.value);
            return hit.point;
        }
    }
}
