using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class StartView : MonoBehaviour
    {
        [SerializeField] private Text m_text = default;
        [SerializeField] private float m_fadeTime = 0.5f;
        [SerializeField] private string m_startString = "START";

        private Coroutine m_coroutine = null;

        private void Start() => m_text.text = null;

        public void SetRestTime(float time)
        {
            // 表示する文字列を決定.
            string text = (time > 0f) ? Mathf.CeilToInt(time).ToString() : m_startString;

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