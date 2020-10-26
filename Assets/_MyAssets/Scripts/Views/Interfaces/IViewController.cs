namespace Views
{
    public interface IViewController
    {
        void SetRestTime(float restTime);
        void SetPlayTime(float playTime);
        void OnGoal();
    }
}
