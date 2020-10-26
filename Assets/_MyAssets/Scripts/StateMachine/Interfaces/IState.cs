namespace StateMachine
{
    public interface IState
    {
        /// <summary>初期化処理</summary>
        void OnStart();

        /// <summary>更新処理</summary>
        /// <returns>継続する場合はtrue</returns>
        bool OnNext();
    }
}