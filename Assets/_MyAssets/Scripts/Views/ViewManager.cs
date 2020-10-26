using UnityEngine;

namespace Views
{
    public class ViewManager : MonoBehaviour, IViewController
    {
        [SerializeField] private StartView m_startView = default;
        [SerializeField] private TimeView m_timeView = default;
        [SerializeField] private GoalView m_goalView = default;

        public void SetRestTime(float restTime) => m_startView.SetRestTime(restTime);
        public void SetPlayTime(float playTime) => m_timeView.SetTime(playTime);
        public void OnGoal() => m_goalView.GoalViewAction();
    }
}
