using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameRules;

namespace View
{ 
    public class TimeView : MonoBehaviour
    {
        [SerializeField] private Text m_text = default;
        [SerializeField] private GameRule m_gameManager = default;
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

