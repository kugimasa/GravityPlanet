using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalView : MonoBehaviour
{
    [SerializeField] Animator _goalAnim;
    [SerializeField] GameObject _goalButton;

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
