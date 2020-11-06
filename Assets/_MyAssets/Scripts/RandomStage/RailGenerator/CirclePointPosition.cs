using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomStage
{
    /// <summary>円上の特定の点のうち、Targetに最も近い位置に移動するコンポーネント</summary>
    /// <remarks>カメラの自動制御用に作成. RailGeneratorFromGoalからの呼び出しを想定</remarks>
    public class CirclePointPosition : MonoBehaviour
    {
        [SerializeField] private Transform m_target = default;
        [SerializeField] private Vector3 m_circleCenter = Vector3.zero;
        [SerializeField] private Vector3 m_circleNormal = Vector3.up;
        [SerializeField] private float m_radius = 10f;
        [SerializeField] private int m_pointNum = 6;

        public Transform Target
        {
            get => m_target;
            set => m_target = value;
        }

        public Vector3 CircleCenter
        {
            get => m_circleCenter;
            set => m_circleCenter = value;
        }

        public Vector3 CircleNormal
        {
            get => m_circleNormal;
            set => m_circleNormal = value;
        }

        private Transform m_transform = default;

        private void Start()
        {
            m_transform = this.transform;
        }

        private void Update()
        {
            if (m_target == null) return;

            var targetPos = m_target.position;
            float minDis = float.MaxValue;
            Vector3 pos = Vector3.zero;
            foreach(var p in GetCirclePoints())
            {
                float dis = Vector3.SqrMagnitude(p - targetPos);
                if(dis <= minDis)
                {
                    pos = p;
                    minDis = dis;
                }
            }
            m_transform.position = pos;
        }

        private void OnDrawGizmos()
        {
            foreach(var pos in GetCirclePoints())
            {
                Gizmos.DrawSphere(pos, 0.1f);
            }
        }

        private IEnumerable<Vector3> GetCirclePoints()
        {
            var rot = Quaternion.LookRotation(m_circleNormal);
            for (int i = 0; i < m_pointNum; i++)
            {
                var theta = 2f * Mathf.PI / m_pointNum * i;
                yield return m_circleCenter + rot * (m_radius * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta)));
            }
        }
    }
}