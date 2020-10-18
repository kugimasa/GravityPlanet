using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Items
{
    public class ItemGenerator_aroundPath : MonoBehaviour
    {
        [SerializeField] GameObject m_itemPrefab = default;
        [SerializeField] CinemachineSmoothPath m_targetPath = default;

        [SerializeField, Range(0, 1)] float m_startItemPos = 0;
        [SerializeField, Range(0, 1)] float m_endItemPos = 1;
        [SerializeField] float m_radius=2;
        [SerializeField] int m_stackCount = 10;
        [SerializeField] int m_stackHoldCount = 10;

        [SerializeField] bool m_createOnAwake = false;

        private void Awake()
        {
            if (m_createOnAwake) GenerateItem();
        }

        [ContextMenu("GenerateItem")]
        private void GenerateItem()
        {
            ItemGenerator.GenerateItem_aroundPath(m_itemPrefab, m_targetPath, m_stackCount,m_stackHoldCount, m_startItemPos, m_endItemPos, m_radius);
        }
    }
}
