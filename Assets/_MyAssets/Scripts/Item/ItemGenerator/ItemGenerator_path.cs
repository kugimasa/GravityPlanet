﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Items
{
    public class ItemGenerator_path : MonoBehaviour
    {

        [SerializeField] GameObject m_itemPrefab;
        [SerializeField] CinemachineSmoothPath m_targetPath;

        [SerializeField, Range(0, 1)] float m_startItemPos=0;
        [SerializeField, Range(0, 1)] float m_endItemPos=1;
        [SerializeField] int m_itemCount=10;

        [SerializeField] bool m_createOnAwake = false;

        private void Awake()
        {
            if (m_createOnAwake) GenerateItem();
        }

        [ContextMenu("GenerateItem")]
        private void GenerateItem()
        {
            ItemGenerator.GenerateItem_withpath(m_itemPrefab, m_targetPath, m_itemCount, m_startItemPos, m_endItemPos);
        }
    }
}
