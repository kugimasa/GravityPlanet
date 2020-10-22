using System;
using GameManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{ 
    public class TimeView : MonoBehaviour
    {
        [SerializeField] private Text m_text = default;
        [SerializeField] private GameManager m_gameManager = default;
        private void Start()
        {
            m_text.text = "00:00:00";
        }

        private void Update()
        {
            m_text.text = m_gameManager.PlayTime;
        }
    }
}

