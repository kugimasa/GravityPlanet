using Cinemachine;
using System.Linq;
using UnityEngine;

namespace RailSystem
{
    [RequireComponent(typeof(CinemachineSmoothPath))]
    public class RailSetter : MonoBehaviour, IRailSetter
    {
        [Header("Path")]
        [SerializeField] private float m_moveSpeed = 10f;
        [SerializeField] private float m_minimumDuration = 1f;

        private float m_pathDuration = 1f;

        private CinemachineSmoothPath Path { get => m_path = m_path ?? GetComponent<CinemachineSmoothPath>(); }
        private CinemachineSmoothPath m_path;

        public Transform GoalPlanet { get; private set; }
        
        /// <summary>惑星fromからtoへのパスを設定</summary>
        /// <remarks>スタート地点がローカル座標の原点となるように実装</remarks>
        public void SetWayPoints(Vector3[] points, Transform goalPlanet)
        {
            CinemachineSmoothPath.Waypoint[] wayPoints = null;
            if(points?.Length > 0)
            {
                var start = points[0];
                transform.position = start;
                wayPoints = points.Select(x => new CinemachineSmoothPath.Waypoint() { position = x - start }).ToArray();
            }

            Path.m_Waypoints = wayPoints;
            Path.InvalidateDistanceCache();
            m_pathDuration = Mathf.Max(Path.PathLength / m_moveSpeed, m_minimumDuration);
            GoalPlanet = goalPlanet;
        }

        public float GetRate(float time)
        {
            return Mathf.Clamp01(time / m_pathDuration);
        }

        public Vector3 GetRailPosition(float rate)
        {
            return Path.EvaluatePositionAtUnit(rate, CinemachinePathBase.PositionUnits.Normalized);
        }
    }
}