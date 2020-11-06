using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomStage;

namespace Items
{
    public class ItemGenerator_point : MonoBehaviour, IItemGenerator
    {
        [SerializeField] GameObject m_itemPrefab=default;
        [SerializeField] Transform m_targetPlanet = default;
        [SerializeField] Transform m_pointParent = default;


        [SerializeField] bool m_createOnAwake = false;

        private void Awake()
        {
            if (m_createOnAwake) GenerateItem();
        }
        [ContextMenu("GenerateItem")]
        public void GenerateItem()
        {
            var items=ItemGenerator.GenerateItem_point(m_itemPrefab, m_pointParent);
            foreach (var item in items) ItemGenerator.DirectToPlanet(item.transform, m_targetPlanet);
        }
    }
}