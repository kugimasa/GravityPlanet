﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ItemGenerator_planet : MonoBehaviour
    {
        [SerializeField] GameObject m_itemPrefab = default;
        [SerializeField] SphereCollider m_targetPlanet = default;
        [SerializeField] float m_distanceFromGround = 1.0f;
        [SerializeField] int m_createCount=1;

        [SerializeField] bool m_createOnAwake = false;

        private void Awake()
        {
            if (m_createOnAwake) GenerateItem();
        }

        [ContextMenu("GenerateItem")]
        private void GenerateItem()
        {
            StartCoroutine(ItemGenerator.WaitFrameAction(1, () =>
            {
                var items = ItemGenerator.GenerateItem_onPlanet_random(m_itemPrefab, m_targetPlanet, m_createCount, m_distanceFromGround);
                foreach (var item in items) ItemGenerator.DirectToPlanet(item.transform, m_targetPlanet.transform);
            }));
        }
    }
}
