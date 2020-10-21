using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public class TimeController : MonoBehaviour
    {
        private float m_currentTime;
        private int m_minutes;
        private int m_seconds;
        private int m_milliseconds;
        private string m_timeText;
        private void Start()
        {
            InitializeTime();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateTime();
            Debug.Log(m_timeText);
        }

        /// <summary>
        /// 経過時間を更新するための関数
        /// Update関数内で呼び出す
        /// </summary>
        private void UpdateTime()
        {
            m_currentTime += Time.deltaTime;
            m_minutes = (int)(m_currentTime / 60);
            m_seconds = (int)(m_currentTime % 60);
            m_milliseconds = (int)((m_currentTime * 100)%100);
            m_timeText = $"{m_minutes:00}:{m_seconds:00}:{m_milliseconds:00}";
        }
        
        /// <summary>
        /// 現在の経過時間を mm:ss:ff 形式のstringで取得するためのGetter関数
        /// </summary>
        /// <returns></returns>
        public string GetTimeText()
        {
            return m_timeText;
        }

        /// <summary>
        /// 経過時間と経過時間のstringを初期化する関数
        /// </summary>
        public void InitializeTime()
        {
            m_currentTime = 0.0f;
            m_timeText = "00:00:00";
        }

    }
    
}
