using GameRules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class StartView : MonoBehaviour
    {
        [SerializeField] private Text m_text = default;
        [SerializeField] private float m_fadeTime = 0.5f;
        [SerializeField] private GameRule m_gameManager = default;

        private Coroutine m_coroutine = null;

        private void Start()
        {
            m_text.text = null;
        }

        private void Update()
        {
            SetRestTime(m_gameManager.StartRestTime);
        }

        private void SetRestTime(float restTime)
        {
            // 表示する文字列を決定.
            string text = "START";
            if (restTime > 0f)
            {
                text = Mathf.CeilToInt(restTime).ToString();
            }

            // 文字の更新がない場合は処理を呼ばない.
            if (text == m_text.text) return;

            if (m_coroutine != null) StopCoroutine(m_coroutine);
            m_coroutine = StartCoroutine(TextCoroutine(text));
        }

        private IEnumerator TextCoroutine(string text)
        {
            Color color = m_text.color;
            float timer = 0f;

            // テキストを表示.
            color.a = 1f;
            m_text.text = text;
            m_text.color = color;

            // テキストをフェード.
            while (timer < m_fadeTime)
            {
                timer += Time.deltaTime;
                color.a = Mathf.Lerp(1f, 0f, timer / m_fadeTime);
                m_text.color = color;
                yield return null;
            }

            // テキストを消す.
            color.a = 0f;
            m_text.color = color;
        }
    }
}