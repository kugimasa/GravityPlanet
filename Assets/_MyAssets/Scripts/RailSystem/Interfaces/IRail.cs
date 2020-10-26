using System;
using UnityEngine;

namespace RailSystem
{
    public interface IRail
    {
        /// <param name="target">動かす対象</param>
        void Move(Transform target);

        /// <remarks>引数のTransformは移動先の惑星</remarks>
        event EventHandler<Transform> OnMoveFinished;
    }
}
