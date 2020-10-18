using PlayerMovement;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RandomStage
{
    public class RailFactory : MonoBehaviour
    {
        [Serializable]
        private class RailGeneratorWeights
        {
            [Range(1f, 100f)] public float m_weight = 1f;
            public RailGenerator m_prefab = default;
        }

        [SerializeField] private Transform m_player = default;
        [SerializeField] private RailGeneratorWeights[] m_rails = default;

        private float m_weightTotal = default;

        private void Awake()
        {
            m_weightTotal = m_rails.Sum(x => x.m_weight);
        }

        public Rail Create(Transform from, Transform to)
        {
            var prefab = GetPrefabRandomly();
            var generator = Instantiate(prefab);
            generator.Initialize(from, to, m_player);
            return generator.Rail;
        }

        /// <summary>重み付けに従って生成するプレハブを選択する</summary>
        /// <remarks>【例外】設定が間違っている場合はArgumentException</remarks>
        private RailGenerator GetPrefabRandomly()
        {
            float w = Random.Range(0f, m_weightTotal);
            float sum = 0f;
            foreach (var rail in m_rails)
            {
                sum += rail.m_weight;
                if (w <= sum) return rail.m_prefab;
            }

            // 以下が実行される場合は設定ミスなので例外を投げる.
            throw new ArgumentException($"Something wrong: maybe Settings of RailGenerator, Sum of Weights - {nameof(this.name)}");
        }
    }
}