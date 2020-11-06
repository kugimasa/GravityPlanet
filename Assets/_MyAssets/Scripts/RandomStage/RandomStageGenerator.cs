using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
namespace RandomStage
{
    [RequireComponent(typeof(RailFactory))]
    public class RandomStageGenerator : MonoBehaviour
    {
        [Header("Planets")]
        [SerializeField] private Transform m_startPlanet = default;
        [SerializeField] private Transform m_goalPlanet = default;

        [Header("Creation")]
        [SerializeField] private int m_planetCounts = 10;
        [SerializeField] private Vector2 m_noiseScaleParallel = new Vector2(-10f, 10f);
        [SerializeField] private Vector2 m_noiseScalePerpendicular = new Vector2(0f, 10f);
        
        [Header("Prefabs")]
        [SerializeField] private GameObject m_planetPrefab = default;

        private RailFactory m_railFactory = default;

        private void Start()
        {
            m_railFactory = GetComponent<RailFactory>();
            List<Transform> planets = CreatePlanets(m_startPlanet, m_goalPlanet);
            CreateRails(planets);
            //追記
            foreach(var planet in planets)
            {
                if (planet.TryGetComponent<IItemGenerator>(out var ig))ig.GenerateItem();
            }
        }

        private void CreateRails(List<Transform> planets)
        {
            Transform prePlanet = null;
            foreach (var nextPlanet in planets)
            {
                if (prePlanet != null)
                {
                    var rail=m_railFactory.Create(prePlanet, nextPlanet);
                    //追記
                    if (rail.TryGetComponent<IItemGenerator>(out var ig)) ig.GenerateItem();
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
            planets.Add(m_startPlanet);
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
            planets.Add(m_goalPlanet);
            
            return planets;
        }
    }
}