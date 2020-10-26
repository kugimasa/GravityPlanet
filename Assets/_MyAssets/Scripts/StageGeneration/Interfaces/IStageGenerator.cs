using UnityEngine;

namespace StageGeneration
{
    public interface IStageGenerator
    {
        /// <summary>惑星startから惑星goalまで繋げたステージを生成する</summary>
        /// <remarks>生成した惑星とパスをタプルで返す</remarks>
        void Create(Transform start, Transform goal);
    }
}
