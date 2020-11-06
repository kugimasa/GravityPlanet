using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Items
{
    public class ItemGenerator_circle : MonoBehaviour
    {
        [SerializeField] GameObject m_itemPrefab = default;

        [SerializeField] int m_itemCount = 10;
        [SerializeField] float m_radiaus = 5.0f;

        [SerializeField] bool m_createOnAwake = false;

        private void Awake()
        {
            if (m_createOnAwake) GenerateItem();
        }

        [ContextMenu("GenerateItem")]
        private void GenerateItem()
        {
            ItemGenerator.GenerateItem_circle(m_itemPrefab, transform.position,m_radiaus, m_itemCount, transform.up);
        }
    }
}