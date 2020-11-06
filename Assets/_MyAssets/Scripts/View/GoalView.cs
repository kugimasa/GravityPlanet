using GameRules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace View
{
    public class GoalView : MonoBehaviour
    {
        [SerializeField] Animator _goalAnim;
        [SerializeField] GameObject _goalButton;

        GameRule gameRule;

        private void Awake()
        {
            gameRule = FindObjectOfType<GameRule>();
        }

        private void Update()
        {
            if (gameRule.NowState == GameRule.State.End)
            {
                GoalViewAction();
            }
        }

        [ContextMenu("GoalView")]
        public void GoalViewAction()
        {
            _goalAnim.SetBool("Goal", true);
        }

        public void Active_retryButton()
        {
            _goalButton.SetActive(true);
        }

        [ContextMenu("Retry")]
        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}