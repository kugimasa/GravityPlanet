using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using RandomStage;

namespace Items
{
    public class ItemGenerator_withPath : MonoBehaviour, IItemGenerator
    {

        [SerializeField] GameObject m_itemPrefab=default;
        [SerializeField] CinemachineSmoothPath m_targetPath = default;

        [SerializeField, Range(0, 1)] float m_startItemPos=0;
        [SerializeField, Range(0, 1)] float m_endItemPos=1;
        [SerializeField] int m_itemCount=10;

        [SerializeField] bool m_createOnAwake = false;

        private void Awake()
        {
            if (m_createOnAwake) GenerateItem();
        }

        [ContextMenu("GenerateItem")]
        public void GenerateItem()
        {
            ItemGenerator.GenerateItem_withPath(m_itemPrefab, m_targetPath, m_itemCount, m_startItemPos, m_endItemPos);
        }
    }
}
