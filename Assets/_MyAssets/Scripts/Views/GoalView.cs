using UnityEngine;
using UnityEngine.SceneManagement;

namespace Views
{
    public class GoalView : MonoBehaviour
    {
        [SerializeField] private Animator m_goalAnim = default;
        [SerializeField] private GameObject m_goalButton = default;

        [ContextMenu("GoalView")]
        public void GoalViewAction()
        {
            m_goalAnim.SetBool("Goal", true);
        }

        public void Active_retryButton()
        {
            m_goalButton.SetActive(true);
        }

        [ContextMenu("Retry")]
        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}