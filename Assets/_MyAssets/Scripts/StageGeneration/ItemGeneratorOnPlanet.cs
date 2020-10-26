using GravitySystem;
using ItemSystem;
using RailSystem;
using System.Collections.Generic;
using UnityEngine;

namespace StageGeneration
{
    public class ItemGeneratorOnPlanet : ItemGenerator
    {
        [SerializeField] private GameObject m_itemPrefabWithPlanetSetter = default;
        [SerializeField] float m_distanceFromGround = 1.0f;
        [SerializeField] Vector2Int m_countRange = new Vector2Int(1, 10);

        public override IEnumerable<IItem> Create(IEnumerable<Transform> planets, IEnumerable<IRail> paths)
        {
            if (m_itemPrefabWithPlanetSetter?.GetComponent<IItem>() == null) yield break;

            foreach (var planet in planets)
            {
                if (!planet.TryGetComponent(out SphereCollider sphere)) continue;

                int count = Random.Range(m_countRange.x, m_countRange.y);
                var items = CreateItems(m_itemPrefabWithPlanetSetter, sphere, count, m_distanceFromGround);
                foreach (var item in items)
                {
                    item.GetComponent<IPlanetSetter>()?.SetPlanet(planet);
                    yield return item.GetComponent<IItem>();
                }
            }
        }

        private IEnumerable<GameObject> CreateItems (GameObject itemPrefab, SphereCollider targetPlanet, int count, float distanceFromGround)
        {
            for (int i = 0; i < count; i++)
            {
                var pos = RandomPosition(targetPlanet, distanceFromGround);
                yield return Instantiate(itemPrefab, pos, Quaternion.identity);
            }
        }

        private Vector3 RandomPosition(SphereCollider targetPlanet, float distanceFromGround)
        {
            var planetTransform = targetPlanet.transform;
            var center = planetTransform.position;
            var posx = Random.Range(-1.0f, 1.0f);
            var posy = Random.Range(-1.0f, 1.0f);
            var posz = Random.Range(-1.0f, 1.0f);
            var randPos = new Vector3(posx, posy, posz).normalized;
            var gravityDirection = randPos;
            //完全な球体のみを想定
            var pos = center + gravityDirection * (planetTransform.localScale.x * targetPlanet.radius + distanceFromGround);
            return pos;
        }
    }
}