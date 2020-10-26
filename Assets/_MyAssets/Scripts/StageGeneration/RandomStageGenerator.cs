using System.Collections.Generic;
using UnityEngine;
using RailSystem;
using System.Linq;
using GravitySystem;

namespace StageGeneration
{
    public class RandomStageGenerator : MonoBehaviour, IStageGenerator
    {
        [Header("Creation")]
        [SerializeField] private int m_planetCounts = 10;
        [SerializeField] private Vector2 m_noiseScaleParallel = new Vector2(-10f, 10f);
        [SerializeField] private Vector2 m_noiseScalePerpendicular = new Vector2(0f, 10f);

        [Header("Factories")]
        [SerializeField] private RailFactory m_railFactory = default;
        [SerializeField] private ItemFactory m_itemFactory = default;

        [Header("Prefabs")]
        [SerializeField] private GameObject m_planetPrefab = default;

        public IPlanetSetter OnRailMove { get; set; }

        public void Create(Transform start, Transform goal)
        {
            var planets = CreatePlanets(start, goal).ToArray();
            
            var rails = CreateRails(planets, m_railFactory).ToArray();
            foreach (var rail in rails) 
            {
                rail.OnMoveFinished += OnRailMoveFinished;
            }

            var items = m_itemFactory?.Create(planets, rails)?.ToArray();
        }

        private void OnRailMoveFinished(object rail, Transform nextPlanet)
        {
            OnRailMove?.SetPlanet(nextPlanet);
        }

        /// <remarks>yield returnを用いた遅延実行なので注意</remarks>
        /// <returns>railFactoryがnullの場合はyield break</returns>
        private IEnumerable<IRail> CreateRails(IEnumerable<Transform> planets, RailFactory railFactory)
        {
            if (railFactory == null) yield break;

            Transform prePlanet = null;
            foreach (var nextPlanet in planets)
            {
                if (prePlanet != null)
                {
                    yield return railFactory.Create(prePlanet, nextPlanet);
                }
                prePlanet = nextPlanet;
            }
        }

        /// <remarks>starPlanetとendPlanet間を等間隔に分割し、そこを基準に乱数でずらして生成</remarks>
        /// <returns>リスト（startPlanet, 生成したPlanet, endPlanet）</returns>
        private List<Transform> CreatePlanets(Transform startPlanet, Transform endPlanet)
        {
            List<Transform> planets = new List<Transform>(); // Rail作成用にPlanetをリストで保持しておく.

            // ベースは等間隔で直線的.
            var startPos = startPlanet.position;
            var dr = (endPlanet.position - startPos) / (m_planetCounts + 1);
            var direction = dr.normalized;
            
            // 生成処理.
            planets.Add(startPlanet);
            for (int i = 0; i < m_planetCounts; i++)
            {
                // 基本位置
                var pos = startPos + (i + 1) * dr;

                // 乱数で基本位置からずらす.
                pos += Random.Range(m_noiseScaleParallel.x, m_noiseScaleParallel.y) * direction;
                var r = Random.Range(m_noiseScalePerpendicular.x, m_noiseScalePerpendicular.y);
                var theta = Random.Range(0f, 2 * Mathf.PI);
                pos += Quaternion.LookRotation(direction) * new Vector3(r * Mathf.Cos(theta), r * Mathf.Sin(theta), 0f);

                // 生成.
                var instance = Instantiate(m_planetPrefab, pos, Quaternion.identity);
                planets.Add(instance.transform);
            }
            planets.Add(endPlanet);
            
            return planets;
        }
    }
}