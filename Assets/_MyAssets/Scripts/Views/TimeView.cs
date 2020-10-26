using UnityEngine;
using UnityEngine.UI;

namespace Views
{ 
    public class TimeView : MonoBehaviour
    {
        [SerializeField] private Text m_text = default;
        
        public void SetTime(float time)
        {
            var min = (int)(time / 60);
            var sec = (int)(time % 60);
            var msec = (int)((time * 100) % 100);
            m_text.text = $"{min:00}:{sec:00}:{msec:00}";
        }

        private void Start() => SetTime(0f);
    }
}
